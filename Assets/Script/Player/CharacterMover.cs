using UnityEngine;

namespace Character
{
    //キャラクターの基本移動を行うクラス
    public class CharacterMover : MonoBehaviour
    {
        [SerializeField] float CharacterMoveSpeed = 10.0f;  //プレイヤーキャラクタの移動速度
        CharacterController characterController;
        Vector2 modeDir;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
        }

        //移動方向取得
        public void SetMove(Vector2 _inputValue)
        {
            // Vector2 で入力値を取得 (x=左右, y=前後)

           // if (_inputValue == Vector2.zero) return;
            modeDir = _inputValue;
        }

        void Update()
        {
            UpdateMove();
        }


        //移動の更新
        public void UpdateMove()
        {
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;

                // 水平面に投影
            forward.y = 0f;
            right.y = 0f;
             //正規化
            forward.Normalize();
            right.Normalize();

            //移動量
            Vector3 moveAmount = (right * modeDir.x + forward * modeDir.y) * CharacterMoveSpeed * Time.deltaTime;
            //実際にキャラクターを動かす
            characterController.Move(moveAmount);

        }
    }
}

