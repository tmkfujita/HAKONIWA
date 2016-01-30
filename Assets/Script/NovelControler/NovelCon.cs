using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// uGUIの機能を使うお約束
using System.IO;
using System.Collections.Generic;

public class NovelCon : MonoBehaviour {

	public string[] scenarios; 		// シナリオを格納する
	public string[] characters; 	// 会話中のキャラクタ名格納する
	public string[] backImages; 	// 背景画像の格納
	public string[]	leftImages; 	// 左側画像の格納
	public string[] rightImages; 	// 右側画像の格納
	public string[]	leftIcons; 		// 左側アイコンの格納
	public string[] rightIcons; 	// 右側アイコンの格納
	public string[]	centerImages; 	// 中央画像画格納する
	public string[]	bgms; 			// BGM


	//UI
	public Text talk;				// uiTextへの参照を保つ
	public Text	nameText;			// uiTextへの参照を保つ
	public RawImage leftImage; 		//左のキャラクタ表示部
	public RawImage rightImage; 	//右のキャラクタ表示部
	public RawImage leftIcon; 		//左のアイコン表示部
	public RawImage rightIcon; 		//右のアイコン表示部
	public RawImage backImage; 		//背景？
	public RawImage effect; 		//エフェクト用？
	public RawImage centerImage; 		//中央画像？
	public MeshRenderer back; 		//360背景？

	public RawImage tobe; 		//エフェクト用？
	public RawImage next;

	int currentLine = 0; 			// 現在の行番号
	public string CurrentText = "";	//表示する会話
	public string CurrentName = "";	//会話中のキャラクター名


	[SerializeField][Range(0.001f, 0.3f)]
	float intervalForCharacterDisplay = 0.1f;

	private string currentText = string.Empty;
	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;
	private int lastUpdateCharacter = -1;
	private int lastUpdateEffect = 100;


	Texture imgtmp;					//テクスチャの一時保管
	Texture tmpleftImage;			//テクスチャの一時保管
	Texture tmprightImage;			//テクスチャの一時保管
	Texture tmpbackImage;			//テクスチャの一時保管
	Texture tmpcenterImage;			//テクスチャの一時保管
	Texture tmprightIcon;			//テクスチャの一時保管
	Texture tmpleftIcon;			//テクスチャの一時保管
	string 	tmpBGM;					//BGM名の一時保管


	//
	int backImageFlag 	= 0;		//イメージの変更フラグ
	int rightImageFlag 	= 0;		//イメージの変更フラグ
	int leftImageFlag 	= 0;		//イメージの変更フラグ
	int effectFlag		= 1;		//エフェクトフラグ
	int centerImageFlag	= 0;		//エフェクトフラグ
	int rightIconFlag	= 0;		//iconフラグ
	int leftIconFlag	= 0;		//iconフラグ
	int bgmFlag			= 0;		//BGMフラグ
	int nextSceneFlag   = 0;
	int bgmFlagStop			= 0;		//BGMフラグ

	string nextScene ="";

	private float time;
	public float fadetime;


	// 文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText 
	{
		get { return  Time.time > timeElapsed + timeUntilDisplay; }
	}
		
	void Start()
	{
		Debug.Log ("NovelCon Start"+DataCon.GetCurrentLine());


		//会話設定ファイルの一時保存用
		List<string> tmpscenarios = new List<string>();
		List<string> tmpcharanamelist = new List<string>();
		List<string> tmpbackImageslist = new List<string>();
		List<string> tmpleftImageslist = new List<string>();
		List<string> tmprightImageslist = new List<string>();
		List<string> tmpleftIconslist = new List<string>();
		List<string> tmprightIconslist = new List<string>();
		List<string> tmpcenterImageslist = new List<string>();
		List<string> tmpBGMlist = new List<string>();


		//会話設定ファイルの読み込み
		TextAsset csv = Resources.Load("CSV/sample") as TextAsset;
		StringReader reader = new StringReader(csv.text);
		while (reader.Peek() > -1) {
			string line = reader.ReadLine();
			string[] values = line.Split(',');
			CurrentText = values [1];
			Debug.Log ("CSVText@."+line);
			Debug.Log ("values@."+values);

			tmpscenarios.Add(StringCheck(values[1].Replace("@@","\n")));
			tmpcharanamelist.Add(StringCheck(values[0]));
			tmpbackImageslist.Add (StringCheck(values[2]));
			tmpleftImageslist.Add (StringCheck(values[4]));
			tmprightImageslist.Add (StringCheck(values[6]));
			tmpleftIconslist.Add (StringCheck(values[3]));
			tmprightIconslist.Add (StringCheck(values[7]));
			tmpcenterImageslist.Add (StringCheck(values[5]));
			tmpBGMlist.Add (StringCheck(values[8]));
		
		}

		//いろいろ格納
		scenarios 	= tmpscenarios.ToArray ();
		characters	= tmpcharanamelist.ToArray ();
		backImages 	= tmpbackImageslist.ToArray ();
		leftImages 	= tmpleftImageslist.ToArray ();
		rightImages = tmprightImageslist.ToArray ();
		leftIcons	= tmpleftIconslist.ToArray(); 		
		rightIcons	= tmprightIconslist.ToArray(); 
		centerImages= tmpcenterImageslist.ToArray(); 	
		bgms		= tmpBGMlist.ToArray(); 		


		print ("scenarios"+scenarios.Length);
		print ("characters"+characters.Length);
		print ("backImages"+backImages.Length);
		print ("leftImages"+leftImages.Length);
		print ("rightImages"+rightImages.Length);



		print(Application.dataPath);
		//リストの解放
		tmpscenarios.Clear();
		tmpcharanamelist.Clear();
		tmpbackImageslist.Clear();
		tmpleftImageslist.Clear();
		tmprightImageslist.Clear();

		SetNextLine();
	}


	void Update () 
	{
		if(lastUpdateEffect > 0 && effectFlag == 1){//なんちゃってフェードいん
			effect.color = new Color(0, 0, 0, (float)lastUpdateEffect/100);
			lastUpdateEffect--;
			Debug.Log("effect"+lastUpdateEffect);
			if (lastUpdateEffect <= 0) {
				effectFlag = 0;
			}
		}else if(effectFlag == 2){//なんちゃってフェードアウト
			effect.color = new Color(0, 0, 0, (float)lastUpdateEffect/100);
			lastUpdateEffect++;
			Debug.Log("effect"+lastUpdateEffect);
			if (lastUpdateEffect >= 100) {
				effectFlag = 0;
				//背景画像の読み込み
				//画像の切り替え処理
				if(backImageFlag > 0){
					Debug.Log("backImage");

					backImage.texture = tmpbackImage;
					tmpbackImage = null;
					backImageFlag = 0;
				}

				if(rightImageFlag > 0){
					rightImage.texture = tmprightImage;
					tmprightImage = null;
					rightImageFlag = 0;
				}

				if(leftImageFlag > 0){
					leftImage.texture = tmpleftImage;
					tmpleftImage = null;
					leftImageFlag = 0;
				}

				if(leftIconFlag > 0){
					leftIcon.texture = tmpleftIcon;
					tmpleftIcon = null;
					leftIconFlag = 0;
				}

				if(rightIconFlag > 0){
					rightIcon.texture = tmprightIcon;
					tmprightIcon = null;
					rightIconFlag = 0;
				}

				if(centerImageFlag > 0){
					centerImage.texture = tmpcenterImage;
					tmpcenterImage = null;
					centerImageFlag = 0;
				}
			}


				

			//背景画像の読み込み
//			if (!backImages [DataCon.GetCurrentLine()].Equals ("")) {
//				if (!backImages [DataCon.GetCurrentLine()].Equals ("0")) {
//					try {
//						tmpbackImage = ReadTexture ("Assets/Resources/img/" + backImages [DataCon.GetCurrentLine()], 1024, 768);
//						print (backImages [DataCon.GetCurrentLine()]+"テクスチャ読み込み完了");
//						backImageFlag++;
//					} catch {
//						print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
//						backImageFlag = 0;
//					}
//				} else {
//					try {
//						tmpbackImage = ReadTexture ("Assets/Resources/img/base.png", 1024, 768);
//						print ("テクスチャ読み込み完了");
//						backImageFlag++;
//					} catch {
//						print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
//						backImageFlag = 0;
//					}
//				}
//			}
			InitChangeImages(DataCon.GetCurrentLine());

		}else if(effectFlag == 3){
			time -= Time.deltaTime;//時間更新(徐々に減らす)
			float a = time / fadetime;//徐々に0に近づける
//			Color color = image.color;//取得したimageのcolorを取得
//			color.a = a;//カラーのアルファ値(透明度合)を徐々に減らす
//			image.color = color;//取得したImageに適応させる
		}

		else{
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) {
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit = new RaycastHit();

				if (Physics.Raycast(ray, out hit)){
					GameObject obj = hit.collider.gameObject;
					Debug.Log(obj.name);
					Debug.Log("Update");
				}
			}
			if (nextSceneFlag > 0) {
				BgmManager.Instance.Stop ();
				nextSceneFlag=0;
				Application.LoadLevel (nextScene);
			}
			//BGM切り替え
			if(bgmFlag > 0){
				BgmManager.Instance.Play (tmpBGM);
				bgmFlag = 0;
				tmpBGM = "";
			}

			if(bgmFlagStop > 0){
				BgmManager.Instance.Stop ();
				bgmFlagStop = 0;
				tmpBGM = "";
			}


			//画像の切り替え処理
			if(backImageFlag > 0){
				Debug.Log("backImage");

				backImage.texture = tmpbackImage;
				tmpbackImage = null;
				backImageFlag = 0;
			}

			if(rightImageFlag > 0){
				rightImage.texture = tmprightImage;
				tmprightImage = null;
				rightImageFlag = 0;
			}

			if(leftImageFlag > 0){
				leftImage.texture = tmpleftImage;
				tmpleftImage = null;
				leftImageFlag = 0;
			}

			if(leftIconFlag > 0){
				leftIcon.texture = tmpleftIcon;
				tmpleftIcon = null;
				leftIconFlag = 0;
			}

			if(rightIconFlag > 0){
				rightIcon.texture = tmprightIcon;
				tmprightIcon = null;
				rightIconFlag = 0;
			}

			if(centerImageFlag > 0){
				centerImage.texture = tmpcenterImage;
				tmpcenterImage = null;
				centerImageFlag = 0;
			}

			// 文字の表示が完了してるならクリック時に次の行を表示する
			if( IsCompleteDisplayText ){
				if((DataCon.GetCurrentLine() < scenarios.Length && Input.GetMouseButtonDown(0)) || (DataCon.GetCurrentLine() < scenarios.Length &&  Input.GetKeyDown("space"))){
					SetNextLine();
				}
			}else{
				// 完了してないなら文字をすべて表示する
				if(Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")){
					timeUntilDisplay = 0;
				}
			}


			int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
			if( displayCharacterCount != lastUpdateCharacter ){
				talk.text = currentText.Substring(0, displayCharacterCount);
				nameText.text = characters [DataCon.GetCurrentLine ()-1];
//				nameText.text = Application.dataPath;
				lastUpdateCharacter = displayCharacterCount;
			}


		}
	}


	void SetNextLine()
	{
		currentText = scenarios[DataCon.GetCurrentLine()];
		InitChangeImages (DataCon.GetCurrentLine());
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		currentLine ++;
		DataCon.SetCurrentLine (currentLine);
		lastUpdateCharacter = -1;
		string tmp = characters[DataCon.GetCurrentLine()];
		print("名前です＠" +tmp);
		if(tmp.Equals("1")){
			effectFlag = 1;
		}else if(tmp.Equals("2")){
			effectFlag = 2;
		}else 
		if(tmp.Contains("@@@")){
			nextScene = tmp.Replace ("@@@", "");
			nextSceneFlag = 1;
			effectFlag = 2;
		}

	}

	void InitChangeImages(int Current){
		//画像を事前に読み込む
		if(!leftImages[Current].Equals("")){
			if (!leftImages [Current].Equals ("0")) {
				try {
//					tmpleftImage = ReadTexture ("Assets/Resources/img/" + leftImages [Current], 500, 500);
					tmpleftImage = Resources.Load("img/"+leftImages [Current])as Texture;

					print ("tmpleftImageテクスチャ読み込み完了");
					leftImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					print ("テクスチャ読み込み失敗@多分" + Application.dataPath + "/Resources/img/" + leftImages [Current]);

					leftImageFlag = 0;
				}
			} else {
				try {
//					tmpleftImage = ReadTexture ("Assets/Resources/img/base.png", 500, 500);
					tmpleftImage = Resources.Load("img/"+leftImages [Current])as Texture;

					print ("テクスチャ読み込み完了");
					leftImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					print ("テクスチャ読み込み失敗@多分" + Application.dataPath + "/Resources/img/" + leftImages [Current]);

					leftImageFlag = 0;
				}
			}
		}

		if (!rightImages [Current].Equals ("")) {
			if(!rightImages [Current].Equals("0")){
				try {
//					tmprightImage = ReadTexture ("Assets/Resources/img/" + rightImages [Current], 500, 500);
					tmprightImage = Resources.Load("img/"+rightImages [Current])as Texture;

					print ("テクスチャ読み込み完了");
					rightImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					rightImageFlag = 0;
				}
			}else{
				try {
//					tmprightImage = ReadTexture ("Assets/Resources/img/base.png", 500, 500);
					tmprightImage = Resources.Load("img/"+rightImages [Current])as Texture;

					print ("テクスチャ読み込み完了");
					rightImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					rightImageFlag = 0;
				}
			}
		}

		if (!backImages [Current].Equals ("")) {
			if (!backImages [Current].Equals ("0")) {
				try {
//					tmpbackImage = ReadTexture ("Assets/Resources/img/" + backImages [Current], 1024, 768);
					tmpbackImage = Resources.Load("img/"+backImages [Current])as Texture;

					print (backImages [Current]+"テクスチャ読み込み完了");
					backImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					backImageFlag = 0;
				}
			} else {
				try {
//					tmpbackImage = ReadTexture ("Assets/Resources/img/base.png", 1024, 768);
					tmpbackImage = Resources.Load("img/"+backImages [Current])as Texture;

					print ("テクスチャ読み込み完了");
					backImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					backImageFlag = 0;
				}
			}

			if(backImages [Current].Equals ("tobecontinued1")){
				Color color = tobe.color;//取得したimageのcolorを取得
				color.a = 0;//カラーのアルファ値(透明度合)を徐々に減らす
				tobe.color = color;//取得したImageに適応させる

			}
		}

		if (!leftIcons [Current].Equals ("")) {
			if (!leftIcons [Current].Equals ("0")) {
				
				try {
					tmpleftIcon = Resources.Load("img/"+leftIcons [Current])as Texture;
//					tmpleftIcon = ReadTexture ("Assets/Resources/img/" + leftIcons [Current], 100, 100);
					print ("テクスチャ読み込み完了");
					leftIconFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					leftIconFlag = 0;
				}
			} else {
				try {
					tmpleftIcon = Resources.Load("img/"+leftIcons [Current])as Texture;
//					tmpleftIcon = ReadTexture ("Assets/Resources/img/base.png", 100, 100);
					print ("テクスチャ読み込み完了");
					leftIconFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					leftIconFlag = 0;
				}
			}
		}

		if (!rightIcons [Current].Equals ("")) {
			if (!rightIcons [Current].Equals ("0")) {
				
				try {
//					tmprightIcon = ReadTexture ("Assets/Resources/img/" + rightIcons [Current], 100, 100);
					tmprightIcon = Resources.Load("img/"+rightIcons [Current])as Texture;

					print ("テクスチャ読み込み完了");
					rightIconFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					rightIconFlag = 0;
				}
			} else {
				try {
//					tmprightIcon = ReadTexture ("Assets/Resources/img/base.png", 100, 100);
					tmprightIcon = Resources.Load("img/"+rightIcons [Current])as Texture;

					print ("テクスチャ読み込み完了");
					rightIconFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					rightIconFlag = 0;
				}
			}
		}

		if (!centerImages [Current].Equals ("")) {
			if (!centerImages [Current].Equals ("")) {
				
				try {
					//					tmpcenterImage = ReadTexture ("Assets/Resources/img/" + centerImages [Current], 500, 500);
					tmpcenterImage = Resources.Load("img/"+centerImages [Current])as Texture;

					print ("テクスチャ読み込み完了");
					centerImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					centerImageFlag = 0;
				}
			} else {
				try {
					tmpcenterImage = Resources.Load("img/"+centerImages [Current])as Texture;

//					tmpcenterImage = ReadTexture ("Assets/Resources/img/base.png", 500, 500);
					print ("テクスチャ読み込み完了");
					centerImageFlag++;
				} catch {
					print ("テクスチャ読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					centerImageFlag = 0;
				}
			}
		}

		//BGM
		if (!bgms [Current].Equals ("")) {
			if (bgms [Current].Equals ("0")) {
				bgmFlagStop++;
			} else {
				try {
					print ("BGM読み込み完了");
					tmpBGM = bgms [Current];
					bgmFlag++;
				} catch {
					print ("BGM読み込み失敗@多分" + DataCon.GetCurrentLine () + "行目だと思う");
					bgmFlag = 0;
				}
			}
		}


	}

	void ChangeImages(){
	//画像の切り替え
	}

	//画像データをバイナリで読み込む
	byte[] ReadPngFile(string path){
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		BinaryReader bin = new BinaryReader(fileStream);
		byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);
		bin.Close();
		return values;
	}

	//テクスチャに変換
	Texture ReadTexture(string path, int width, int height){
		byte[] readBinary = ReadPngFile(path);
		Texture2D texture = new Texture2D(width, height);
		texture.LoadImage(readBinary);
		return texture;
	}

	string StringCheck (string str){
		if ((str != null) && (str.Length != 0))
		{
			// nullではなく、かつ空文字列でもない
			return str;
		}
		else
		{
			// null、もしくは空文字列である
			return "";
		}
	}
		
}



