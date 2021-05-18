using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // ユニティちゃんの走る速さ
    private const float Speed = 3f;

    // ユニティちゃんの回転する速さ
    private const float RotateSpeed = 720f;

    // ユニティちゃん
    [SerializeField]
    private Transform _unityChan;

    // ユニティちゃんのアニメーター
    private Animator _unityChanAnimator;

    // アイテム取得時のコールバック
    public Action OnGetItemCallback;

    // 終了状態かどうか
    private bool _isFinished = false;
    
    // 入力受け付けるか
    [HideInInspector]
    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        _unityChanAnimator = _unityChan.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFinished || !isReady)
        {
            return;
        }

        // キーボード入力を進行方向のベクトルに変換して返す
        Vector3 direction = InputToDirection();

        // 進行方向のベクトルの大きさ
        float magnitude = direction.magnitude;

        // 進行方向のベクトルが移動量を持っているかどうか
        if (Mathf.Approximately(magnitude, 0f) == false)
        {
            _unityChanAnimator.SetBool("running", true);
            UpdatePosition(direction);
            UpdateRotation(direction);
        }
        else
        {
            _unityChanAnimator.SetBool("running", false);
        }
    }

    // キーボード入力を進行方向のベクトルに変換して返す
    private Vector3 InputToDirection()
    {
        Vector3 direction = new Vector3(0f, 0f, 0f);

        // 「右矢印」を入力
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction.x += 1f;
        }

        // 「左矢印」を入力
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction.x -= 1f;
        }

        // 「上矢印」を入力
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction.z += 1f;
        }

        // 「下矢印を入力」
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction.z -= 1f;
        }

        return direction.normalized;
    }

    // 位置を更新
    private void UpdatePosition(Vector3 direction)
    {
        Vector3 dest = transform.position + direction * Speed * Time.deltaTime;
        dest.x = Mathf.Clamp(dest.x, -4.7f, 4.7f);
        dest.z = Mathf.Clamp(dest.z, -4.7f, 4.7f);
        transform.position = dest;
    }

    // 方向を更新
    private void UpdateRotation(Vector3 direction)
    {
        Quaternion from = _unityChan.rotation;
        Quaternion to = Quaternion.LookRotation(direction);
        _unityChan.rotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);
    }

    // ほかのトリガイベントに侵入した際に呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();
            item.Gotten();
            OnGetItemCallback();
        }
    }

    // 終了処理
    public void Finish()
    {
        _isFinished = true;
        _unityChan.rotation = Quaternion.Euler(0f, 180f, 0f);
        _unityChanAnimator.SetBool("finish", true);
    }
}
