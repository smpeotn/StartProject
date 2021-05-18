using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Game : MonoBehaviour
{
    // 生成するアイテムの総数
    public const int Total = 10;

    private bool isReady = false;
    
    // 残数
    private int _restCount;

    // 残数のテキスト
    [SerializeField]
    private Text _restCountText;

    // プレイヤー
    [SerializeField]
    private Player _player;

    // アイテムのPrefab
    [SerializeField]
    private GameObject _itemPrefab;

    // CLEARの画像
    [SerializeField]
    private Image _clearImage;

    // 「もう一度」ボタン
    [SerializeField]
    private Button _restartButton;

    [SerializeField]
    private Image _readyImg;

    [SerializeField] 
    private CinemachineVirtualCamera vcam;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // アイテムを生成
    private void CreateItems()
    {
        for (int i = 0; i < Total; i++)
        {
            GameObject item = Instantiate(_itemPrefab);
            item.transform.position = GetRandomItemPosition();
        }
    }

    // ランダムにアイテムを配置する座標を返す
    private Vector3 GetRandomItemPosition()
    {
        // 1f～3.5fの間でランダムにX座標を決定
        var x = UnityEngine.Random.Range(1f, 3.5f);
        // 1/2の確率で反転
        if (UnityEngine.Random.Range(0, 2) % 2 == 0)
        {
            x *= -1f;
        }

        // 1f～3.5fの間でランダムにZ座標を決定
        var z = UnityEngine.Random.Range(1f, 3.5f);
        // 1/2の確率で反転
        if (UnityEngine.Random.Range(0, 2) % 2 == 0)
        {
            z *= -1f;
        }

        return new Vector3(x, 0f, z);
    }

    // 残りアイテム数を設定
    private void SetRestCount(int value)
    {
        _restCount = value;
        _restCountText.text = string.Format("残り{0}個", _restCount);
    }

    // アイテム取得時の処理
    private void OnGetItem()
    {
        SetRestCount(_restCount - 1);
        if (_restCount == 0)
        {
            Finish();
        }
    }

    // 終了処理
    private void Finish()
    {
        _player.Finish();
        _clearImage.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
    }

    // もう一度遊ぶ処理
    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Ready()
    {
        _readyImg.gameObject.SetActive(true);
        vcam.Priority = 1;
        SetRestCount(Total);
        CreateItems();
        _player.OnGetItemCallback = OnGetItem;
    }

    public void GameStart()
    {
        _readyImg.gameObject.SetActive(false);
        _player.isReady = true;

    }
}
