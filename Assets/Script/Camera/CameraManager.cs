using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;


namespace Manager
{
    public class CameraManager : SingletonBase<CameraManager>
    {
        [Header("Settings")]
        [SerializeField] float sensitivityX; // 水平感度
        [SerializeField] float sensitivityY; // 垂直感度
        [SerializeField] const float MIN_PITCH = -85f;     // 視点下限
        [SerializeField] const float MAX_PITCH = 85f;      // 視点上限
        float pitch = 0; // カメラの上下角（度）

        const int MAX_PRIORITY = 1;
        const int MIN_PRIORITY = 0;
        [SerializeField] CinemachineCamera exploreCam;
        //[SerializeField] CinemachineCamera conversationCam;

        [SerializeField] Transform player;
        [SerializeField] Transform exploreCamTransform;        // 縦回転を担当（=Main Camera）
        //[SerializeField] Transform conversationCamTransform;
        CameraType defaultCameraType = CameraType.EXPLORE;


        public enum CameraType
        {
            EXPLORE,
            CONVERSATION
        }


        Dictionary<CameraType, CinemachineCamera> cameraDictionary;

        // ゲーム開始時に呼ばれる
        void Start()
        {
            // カーソルを非表示＆ロック
            Cursor.lockState = CursorLockMode.Locked;   // カーソルを画面中央に固定
            Cursor.visible = false;                     // カーソルを非表示にする

            cameraDictionary = new Dictionary<CameraType, CinemachineCamera>();

            //カメラを登録
            if (exploreCam != null) cameraDictionary[CameraType.EXPLORE] = exploreCam;
            //if (conversationCam != null) cameraDictionary[CameraType.CONVERSATION] = conversationCam;

            ChangeCurrentCam(defaultCameraType);

        }

        void LateUpdate()
        {
            this.transform.position = player.position;
        }
        //回転
        public void RotateExploreCam(Vector2 _inputValue)
        {
            // 回転角度を算出
            float deltaX = _inputValue.x * sensitivityX * Time.deltaTime;
            float deltaY = _inputValue.y * sensitivityY * Time.deltaTime;

            player.Rotate(Vector3.up, deltaX, Space.World); //Y軸を中心に回転 水平回転

            float yaw = player.eulerAngles.y;
                                                            // カメラ（ピッチ）
            pitch -= deltaY;                      // 上を見るとマウスは正→ピッチは減る
            pitch = Mathf.Clamp(pitch, MIN_PITCH, MAX_PITCH); //上下の回転を制限
            exploreCamTransform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
            //conversationCamTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);   //カメラ切り替わった時にガクらないように会話用カメラも一応追従

        }

        public void ChangeCurrentCam(CameraType targetCameraType)
        {
            foreach (var camera in cameraDictionary)
            {
                var camValue = camera.Value;
                if (targetCameraType == camera.Key)
                {
                    camValue.Priority.Value = MAX_PRIORITY; //辞書型のValueとややこしいよね。今回は優先度のValue
                }
                else
                {
                    camValue.Priority.Value = MIN_PRIORITY;
                }

            }
        }

        /// <summary>
        /// 会話時に視点を向けるターゲットを指定
        /// </summary>
        /// <param name="_targetTransform"></param>
        public void SetLookTarget(Transform _targetTransform)
        {
            //conversationCam.LookAt = _targetTransform;
            //conversationCam.Follow = _targetTransform;

            exploreCam.LookAt = _targetTransform;
            exploreCam.Follow = _targetTransform;
        }

        /// <summary>
        /// 会話状態から抜ける時の処理
        /// </summary>
        public void ExitConversationCamHanlder()
        {
            UnSetLookTarget();
            SyncRotationFromCamera();
        }

        /// <summary>
        /// 視点を向ける対象
        /// </summary>
        private void UnSetLookTarget()
        {
            //conversationCam.LookAt = null;
            //conversationCam.Follow = null;

            exploreCam.LookAt = null;
            exploreCam.Follow = null;

        }

        /// <summary>
        ///会話が終わった時にカメラがガクっとなるのを補正
        /// </summary>
        private void SyncRotationFromCamera()
        {
            // 現在のカメラのワールド回転から、ピッチ角(X軸)を抽出して変数に上書きする
            // eulerAngles.x は 0-360度なので、Clampしやすいように -180〜180に変換
            float x = exploreCamTransform.eulerAngles.x;
            if (x > 180) x -= 360;

            pitch = x;

            // プレイヤーのY軸回転も同期させる これをしないと、会話中にNPCを向いた方向に体がついていかない
            Vector3 currentEuler = exploreCamTransform.eulerAngles;
            player.rotation = Quaternion.Euler(0, currentEuler.y, 0);
        }

        /// <summary>
        /// カメラの角度を取得（ビルボードに使用)
        /// </summary>
        private void GetCameraRot()
        {

        }

    }
}

