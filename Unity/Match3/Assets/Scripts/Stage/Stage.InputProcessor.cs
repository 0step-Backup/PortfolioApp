using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LLOYD.Match3
{
    public partial class Stage
    {
        IEnumerator Swap_Gems(Node.Gem __gem1, Node.Gem __gem2)
        {
            var pos1 = __gem1.transform.position;
            var pos2 = __gem2.transform.position;

            __gem1.Move(pos2);
            __gem2.Move(pos1);

            while(__gem1.IsMoving || __gem2.IsMoving)
            {
                yield return null;
            }

            {//DEBUG LOG
                string DBGLOG = $"[Swap_Gems] Before Gems ({_gems.Count}):";
                foreach (var item in _gems)
                {
                    DBGLOG += $"\n\t[{item.Key}] {item.Value}";
                }
                Debug.Log(DBGLOG);
            }

            //변경된 위치로 cell 좌표 업데이트
            var pos_cell1 = _tilemap.WorldToCell(__gem1.transform.position);
            var pos_cell2 = _tilemap.WorldToCell(__gem2.transform.position);

            _gems[pos_cell1] = __gem1;
            _gems[pos_cell2] = __gem2;
            __gem1.Update_Name(pos_cell1);
            __gem2.Update_Name(pos_cell2);

            {//DEBUG LOG
                string DBGLOG = $"[Swap_Gems] After Gems ({_gems.Count}):";
                foreach (var item in _gems)
                {
                    DBGLOG += $"\n\t[{item.Key}] {item.Value}";
                }
                Debug.Log(DBGLOG);
            }
            yield return new WaitForSeconds(0.1f);
        }

        public void Enter_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=red>Stage.Enter_Gem</color>({__gem.transform.position})");

            if (_pickGem
                && _pickGem != __gem)
            {
                var pos_pick = _pickGem.transform.position;
                var pos_target = __gem.transform.position;

                if (!IsAdjacent(_pickGem, __gem))//인접한 블럭 체크
                    return;

                var gem1 = _pickGem;
                var gem2 = __gem;
                _pickGem = null;

                StartCoroutine(Swap_Gems(gem1, gem2));

                ////Debug.Log($"<color=cyan>target gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }
        public void Out_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=grey>Stage.Out_Gem</color>({__gem.transform.position})");
        }

        public void Push_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=green>Stage.Push_Gem</color>({__gem.transform.position})");

            if (!_pickGem)
            {
                _pickGem = __gem;

                //Debug.Log($"<color=green>pick gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }

        public void Release_Gem(Node.Gem __gem)
        {
            //Debug.Log($"<color=black>Stage.Release_Gem</color>({__gem.transform.position})");

            if (_pickGem &&
                _pickGem == __gem)
            {
                _pickGem = null;
                //Debug.Log($"<color=grey>pick release gem: {__gem.TYPE}</color>, ({__gem.transform.position.x}, {__gem.transform.position.y})");
            }
        }
    }
}
