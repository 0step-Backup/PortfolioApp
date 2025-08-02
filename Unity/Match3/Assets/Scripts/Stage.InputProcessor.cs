using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLOYD.Match3
{
    public partial class Stage
    {
        List<Node.Gem> _swap_gems = new List<Node.Gem>();

        IEnumerator Swap_Gems()
        {
            Debug.AssertFormat(2 == _swap_gems.Count, "바꿀게 2개가 아니라고?");

            var pos1 = _swap_gems[0].transform.position;
            var pos2 = _swap_gems[1].transform.position;

            _swap_gems[0].Move(pos2);
            _swap_gems[1].Move(pos1);

            while(_swap_gems[0].IsMoving || _swap_gems[1].IsMoving)
            {
                yield return null;
            }

            //Debug.Log("Exit !!");
            _swap_gems.Clear();
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

                _swap_gems.Add(_pickGem);
                _swap_gems.Add(__gem);
                _pickGem = null;

                StartCoroutine(Swap_Gems());

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
