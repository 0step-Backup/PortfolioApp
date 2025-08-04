using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace LLOYD.Match3
{
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
        IEnumerator Process_ChainReaction()
        {
            bool hasMatches = true;
            int chainCount = 0;

            //Debug.Break();

            //var wfs_remove = new WaitForSeconds(1.5f);

            while (hasMatches)
            {
                chainCount++;
                hasMatches = false;

                //모든 젬의 이동 완료까지 대기
                yield return StartCoroutine(WaitForAll_GemsSettled());

                //매치 찾기
                var matches = FindAll_Matches();

                if (matches.Count > 0)
                {
                    //{//DEV TEST
                    //    string DBGLOG = "maching gems";
                    //    foreach (var gem in matches)
                    //    {
                    //        gem.GetComponent<SpriteRenderer>().color = new Color32(64, 64, 64, 255);

                    //        var pos_cell = _tilemap.WorldToCell(gem.transform.position);
                    //        DBGLOG += $"\n\t[{pos_cell.x}, {pos_cell.y}] {gem}";
                    //    }
                    //    Debug.Log(DBGLOG);

                    //    DBGLOG = "maching ignore gems";
                    //    foreach (var gem in _gems)
                    //        DBGLOG += $"\n\t[{gem.Key.x}, {gem.Key.y}] {gem.Value}";
                    //    Debug.Log(DBGLOG);

                    //    Debug.Break();
                    //}

                    hasMatches = true;
                    Debug.Log($"Chain {chainCount}: Found {matches.Count} matches!");

                    //매치된 Gem 제거
                    //Remove_MatchedGems(matches);
                    yield return new WaitForSeconds(Remove_MatchedGems(matches));
                }
                else
                {
                    //Debug.Log($"Chain completed! Total chains: {chainCount}");
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
    }
}
