=============================================================

[working on]實作傳遞JSON檔案
	add Controllers
		新增IActionResult  Spots
		回傳Json檔案
		設定連線為 POST
		使用PostMan測試 是否可以回傳成功
		因為接收的是JSON格式 Core看不懂 要在接收參數前面加 [FromBody]
	add HomeController
		新增IActionResult  Spots
		新增檢視
	add Spots.cshtml
		建立一個 async function 從API/Spots 取得回傳資料
		呼叫 async function
	edit IActionResult Spots
		實作根據條件篩選回傳資料
	
[V]老師實作Homework2
	add Method/Dtos
		新增MemberDto來接收傳過來的資料
	Edit CheckAccount
		接收name修改成接收MemberDto
	add fetch(url,{...})後面的參數設定
		新增method:"POST",body:Formdata
	add 實作上傳image
	Edit Models/Dtos
		新增IFormFile Avatar(Image) 來承接網頁傳進來的image
	Edit CheckAccount 
		add Path.Combine 取得存放路徑
		add FileStream 上傳檔案
		add memoryStream 儲存到記憶體並取得二進位資料
		add 存到database裡面
		add 回傳新增成功

[V]自己寫Homework2
	add Homework2/View
		新增 index.cshtml 
	add fetch的 QueryString
		新增url: API/CheckAccount+ 輸入的name
	add API/CheckAccount
		新增ActionResult
	Edit CheckAccount
		實作檢查名子是否跟database重複
		回傳 true(重複)/false(沒重複)
	add Homework2 接收 API回傳的資料
		接收回傳資料並判斷顯示在網頁的文字
=============================================================