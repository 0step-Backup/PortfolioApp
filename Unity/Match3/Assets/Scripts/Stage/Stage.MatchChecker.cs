using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace LLOYD.Match3
{
    using Common;

    public partial class Stage
    {
        //bool IsAdjacent(Vector3 pos1, Vector3 pos2) => Vector3.Distance(pos1, pos2) <= 1f;
        bool IsAdjacent(Node.Gem gem1, Node.Gem gem2)
        {
            var pos_cell1 = _tilemap.WorldToCell(gem1.transform.position);
            var pos_cell2 = _tilemap.WorldToCell(gem2.transform.position);

            bool isAdjacent = Mathf.Abs(pos_cell1.x - pos_cell2.x) +
                             Mathf.Abs(pos_cell1.y - pos_cell2.y) == 1;

            //Debug.Log($"Adjacent Check: ({pos_cell1.x},{pos_cell1.y}) vs ({pos_cell2.x},{pos_cell2.y}) = {isAdjacent}");

            return isAdjacent;
        }

        //매칭 연쇄 처리
        IEnumerator Process_ChainReaction(Vector3Int __cellpos1, Vector3Int __cellpos2)
        {
            bool hasMatches = true;
            int chainCount = 0;

            //Debug.Break();

            //var wfs_remove = new WaitForSeconds(1.5f);

            while (hasMatches)
            {
                hasMatches = false;

                //모든 젬의 이동 완료까지 대기
                yield return StartCoroutine(WaitForAll_GemsSettled());

                //매치 찾기
                var matches = FindAll_Matches();

                if (matches.Count > 0)
                {
                    chainCount++;

                    //{//DEV TEST
                    //    string DBGLOG = "maching gems";
                    //    foreach (var gem in matches)
                    //    {
                    //        //gem.GetComponent<SpriteRenderer>().color = new Color32(64, 64, 64, 255);

                    //        var pos_cell = _tilemap.WorldToCell(gem.transform.position);
                    //        DBGLOG += $"\n\t[{pos_cell.x}, {pos_cell.y}] {gem}";
                    //    }
                    //    Debug.Log(DBGLOG);

                    //    DBGLOG = "maching ignore gems";
                    //    foreach (var gem in _gems)
                    //        DBGLOG += $"\n\t[{gem.Key.x}, {gem.Key.y}] {gem.Value}";
                    //    Debug.Log(DBGLOG);

                    //    //Debug.Break();
                    //}

                    hasMatches = true;
                    Debug.Log($"Chain {chainCount}: Found {matches.Count} matches!");

                    //매치된 Gem 제거
                    //Remove_MatchedGems(matches);
                    yield return new WaitForSeconds(Remove_MatchedGems(matches));

                    //Gem 추가 및 낙하
                    yield return StartCoroutine(Drop_Gems());
                }
                else
                {
                    //Debug.Log($"Chain completed! Total chains: {chainCount}");

                    if (0 == chainCount)//매칭된 적이 없을 때. Swap 실패
                    {//되돌리기
                        var pos1 = _gems[__cellpos1].transform.position;
                        var pos2 = _gems[__cellpos2].transform.position;

                        _gems[__cellpos1].Rollback_Swap(pos2);
                        _gems[__cellpos2].Rollback_Swap(pos1);

                        while (_gems[__cellpos1].IsMoving || _gems[__cellpos2].IsMoving)
                            yield return null;

                        var temp = _gems[__cellpos1];
                        _gems[__cellpos1] = _gems[__cellpos2];
                        _gems[__cellpos2] = temp;
                    }
                }
            }
        }

        // 모든 젬의 애니메이션 완료 대기
        IEnumerator WaitForAll_GemsSettled()
        {
            yield return null;
        }

        // 매치된 Gem 제거
        private float Remove_MatchedGems(List<Node.Gem> matchedGems)
        {
            float ret = 0f;
            foreach (var gem in matchedGems)
            {
                if (gem != null)
                {
                    var time = gem.Crash();
                    if (ret < time) ret = time;
                }
            }
            return ret;
        }

        // 매치 검사
        List<Node.Gem> FindAll_Matches()
        {
            var matchedGems = new HashSet<Node.Gem>();

            foreach (var kv in _gems)
            {
                var cell = kv.Key;
                var gem = kv.Value;
                if (gem == null) continue;

                // 가로 매치 검사
                var horiz = new List<Node.Gem> { gem };
                for (int dx = 1; ; dx++)
                {
                    var nextCell = cell + new Vector3Int(dx, 0, 0);
                    if (_gems.TryGetValue(nextCell, out var nextGem) && nextGem != null && nextGem.TYPE == gem.TYPE)
                        horiz.Add(nextGem);
                    else break;
                }
                for (int dx = -1; ; dx--)
                {
                    var nextCell = cell + new Vector3Int(dx, 0, 0);
                    if (_gems.TryGetValue(nextCell, out var nextGem) && nextGem != null && nextGem.TYPE == gem.TYPE)
                        horiz.Add(nextGem);
                    else break;
                }
                if (horiz.Count >= 3)
                {
                    foreach (var g in horiz)
                        matchedGems.Add(g);
                }

                // 세로 매치 검사
                var vert = new List<Node.Gem> { gem };
                for (int dy = 1; ; dy++)
                {
                    var nextCell = cell + new Vector3Int(0, dy, 0);
                    if (_gems.TryGetValue(nextCell, out var nextGem) && nextGem != null && nextGem.TYPE == gem.TYPE)
                        vert.Add(nextGem);
                    else break;
                }
                for (int dy = -1; ; dy--)
                {
                    var nextCell = cell + new Vector3Int(0, dy, 0);
                    if (_gems.TryGetValue(nextCell, out var nextGem) && nextGem != null && nextGem.TYPE == gem.TYPE)
                        vert.Add(nextGem);
                    else break;
                }
                if (vert.Count >= 3)
                {
                    foreach (var g in vert)
                        matchedGems.Add(g);
                }
            }

            if(0 < matchedGems.Count)
            {
                foreach (var item in matchedGems)
                {
                    var finded = _gems.FirstOrDefault((gem) => gem.Value == item);
                    if (null != finded.Value)
                        _gems[finded.Key] = null;

                    item.transform.parent = TRSF_UnderWorld;
                }
            }

            return matchedGems.ToList();
        }

        // 떨어뜨리기
        IEnumerator Drop_Gems()
        {
            bool hasDropped = true;

            // 더 이상 떨어질 Gem이 없을 때까지 반복
            while (hasDropped)
            {
                RegenGem_Empty();//빈Cell 있으면 Gem 추가 생성

                hasDropped = false;
                var movesToExecute = new List<(Vector3Int fromCell, Vector3Int toCell, Node.Gem gem)>();

                Dictionary<int, int> bottomGemPos = new Dictionary<int, int>();
                //낙하 잼의 최하단 좌표

                // 각 열(column)을 아래에서 위로 검사
                foreach (var kv in _gems)
                {
                    var cell = kv.Key;
                    var gem = kv.Value;

                    if (gem == null) continue; // 빈 셀은 건너뛰기

                    // 아래쪽 셀 확인
                    var belowCell = cell + Vector3Int.down;
                    
                    // 아래쪽 셀이 _gems에 있고, 비어있다면 떨어뜨리기
                    if (_gems.ContainsKey(belowCell) && _gems[belowCell] == null)
                    {
                        // 이동할 항목을 리스트에 추가 (아직 실행하지 않음)
                        movesToExecute.Add((cell, belowCell, gem));
                        hasDropped = true;

                        if(bottomGemPos.ContainsKey(belowCell.x))
                        {
                            if (belowCell.y < bottomGemPos[belowCell.x])
                                bottomGemPos[belowCell.x] = belowCell.y;
                        }
                        else
                        {
                            bottomGemPos.Add(belowCell.x, belowCell.y);
                        }
                    }
                }

                // 루프가 끝난 후 실제 이동 실행
                foreach (var move in movesToExecute)
                {
                    bool isBottom_Goal = false;
                    if(bottomGemPos.ContainsKey(move.toCell.x))
                    {
                        if(move.toCell.y == bottomGemPos[move.toCell.x])//최하단 젬이고
                        {
                            var more_down_cell = move.toCell + Vector3Int.down;//더 아래 Cell

                            //잼목록에 없거나(바닥), 더 아래 잼이 있으면 마지막 이동
                            if (!_gems.ContainsKey(more_down_cell) || _gems[more_down_cell])
                                isBottom_Goal = true;
                        }

                        //{ if (isBottom_Goal) Debug.Log($"제일 바닥에 있고 마지막 이동= [{move.toCell}] {move.gem}"); }//DEV TEST
                    }

                    Vector3 targetPos = _tilemap.GetCellCenterWorld(move.toCell);
                    float moveTime = move.gem.Drop(targetPos, isBottom_Goal);

                    // _gems 업데이트
                    _gems[move.fromCell] = null;        // 기존 위치를 null로
                    _gems[move.toCell] = move.gem;      // 새 위치에 Gem 설정
                }

                // Gem 이동 대기시간
                if (hasDropped)
                {
                    yield return new WaitForSeconds(0.075f);
                    //Debug.Break();
                }
            }
            //Debug.Log("Drop_Gems completed!");
        }

        void Complte_RegenGem(Node.Gem __gem)
        {
            __gem.transform.parent = TRSF_Gems;

            //Debug.Log($"Complte_RegenGem({__gem})");
        }

        bool RegenGem_Empty()
        {
            bool ret = false;

            var columns = new Dictionary<int, List<Vector3Int>>();
            foreach (var cell in _regenCells.Keys)
            {
                int x = cell.x;
                if (!columns.ContainsKey(x))
                    columns[x] = new List<Vector3Int>();
                columns[x].Add(cell);
            }

            var data_newgem = new Addon.Tilemap.GemDesignValue() {
                type = Defines.Gem.random,
                pos_wolrd = Vector3Int.zero,
            };

            foreach (var col in columns.Values)
            {
                // y 오름차순(아래에서 위로) 정렬
                col.Sort((a, b) => a.y.CompareTo(b.y));

                foreach (var cell in col)
                {
                    var targetcell = cell + Vector3Int.down;
                    Debug.AssertFormat(_gems.ContainsKey(targetcell), $"키가 없어? ({targetcell.x}, {targetcell.y})");
                    if (_gems[targetcell])
                        continue;

                    // 비어있는 셀에만 새 젬 생성
                    Vector3 spawnPos = _regenCells[cell];
                    Vector3 targetPos = _regenCells[cell] + Vector3.down;
                    data_newgem.pos_wolrd = spawnPos;

                    var gem = Add_Gem(targetcell, data_newgem, Node.Gem.NewType.regen);
                    {
                        _gems[targetcell] = gem;
                        gem.Update_Name(targetcell);
                    }
                    //Debug.Break();

                    gem.Regen(targetPos, Complte_RegenGem);
                    ret = true;
                }
            }
            //{ if (ret) Debug.Break(); }//DEV TEST

            return ret;
        }
    }
}
