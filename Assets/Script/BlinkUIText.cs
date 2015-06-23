using UnityEngine;
using UnityEngine.UI;

///<summary>
///
/// Unity UI（uGUI）のテキストを点滅させるためのスクリプト
/// Unity UIのテキストは、一般的なGameObjectと違って、iTweenでアルファー値を変更する事ができないので自作
///
///</summary>
public class BlinkUIText : MonoBehaviour {

	// アタッチしたText自身
	private Text textField;
	
	// 点滅間隔の設定
	[SerializeField, RangeAttribute(1, 25)]
	int step = 1;
	
	// 最も透明の部分の透明度を設定
	[SerializeField, RangeAttribute(0, 192)]
	public int offsetAlpha = 0;
	
	// 計算コスト削減の為
	float radianBase = Mathf.PI / 180;
	
	// アルファー最大値
	int alpha = 255;
	
	// カウンター
	int counter = 0;
	
	Color32 currentColor;
	
	Color32[] alphaTable = new Color32[180];

	// 計算コスト削減の為にsinの計算を予め行って、配列に格納
	void Start () {		
		// アタッチ中のUI Textへの参照：これを使って、透明度を変更する
		textField = GetComponent<Text>();
		
		// 現状の色を運び屋に渡しておく:uGUIのテキストは直接 rgba にアクセスできないので
		currentColor = textField.color;
		
		// sinを 0 〜 179 まで計算
		for ( int i = 0; i < 180; i++ ) {
			// 0 〜 1 〜 0 までのsinの変化値を 255 にかけて、透明度の増減を求める
			float value = (alpha - offsetAlpha) * Mathf.Sin( i * radianBase );
			// 仲買業者に一旦預ける
			currentColor.a = (byte)( ((int)value) + offsetAlpha );
			// 上の答えを配列に収める
			alphaTable[i] = currentColor;
		}
	}
	
	void Update () {
		// int型の上限値に近くなったら強制的にリセット
		if ( counter > 2147483071 ) counter = 0;
		// 予め計算された配列を使って、透明度を変化させるためのインデックスの計算
		int indx = counter%180;
		// 透明度をアサイン
		textField.color = alphaTable[indx];
		// カウンターを進める：stepの変化値で点滅の調整が出来る
		counter += step;
	}
}
