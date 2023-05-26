$(function(){
    // 產品列表
// [{}, {}, {}, {},...]
const productList = [
    {
        id: '1',
        title: '產品一',
        price: 10,
        img: 'https://picsum.photos/id/999/1200/600',
        tags: ['生活用品', '工具']
    },
    {
        id: '2',
        title: '產品二',
        price: 60,
        img: 'https://picsum.photos/id/1070/1200/600',
        tags: ['藥妝']
    },
    {
        id: '3',
        title: '產品三',
        price: 180,
        img: 'https://picsum.photos/id/1071/1200/600',
        tags: ['食品', '飲料']
    },
    {
        id: '4',
        title: '產品四',
        price: 220,
        img: 'https://picsum.photos/id/1072/1200/600',
        tags: ['生活用品', '文具']
    },
    {
        id: '5',
        title: '產品五',
        price: 360,
        img: 'https://picsum.photos/id/1073/1200/600',
        tags: ['工具']
    },
    {
        id: '6',
        title: '產品六',
        price: 360,
        img: 'https://picsum.photos/id/1074/1200/600',
        tags: ['食品']
    },
    {
        id: '7',
        title: '產品七',
        price: 400,
        img: 'https://picsum.photos/id/1075/1200/600',
        tags: ['生活用品', '工具']
    },
    {
        id: '8',
        title: '產品八',
        price: 450,
        img: 'https://picsum.photos/id/1076/1200/600',
        tags: ['生活用品', '工具']
    },
    {
        id: '9',
        title: '產品九',
        price: 520,
        img: 'https://picsum.photos/id/1077/1200/600',
        tags: ['藥妝', '保養']
    }
];

// 設計渲染商品的函數
function renderProductList() {
    // 透過迴圈將produstList內的資料一一取出
    productList.forEach(product => {
        // console.log('[product]', product);
        // 取得一張卡片的HTML
        const card = createProductCardElement(product);
        // console.log('[card]', card);
        // document.getElementById("productRow").innerHTML += card;
        $("#productRow").append(card);
    });
}

// 設計建立單一商品卡片HTML標籤的函數
function createProductCardElement(product) {
    // 產生一個Bootstrap Card的元件
    // https://getbootstrap.com/docs/4.4/components/card/
    const cardElement = `
        <div class="col-md-4">
            <div class="card mb-3">
                <img src="${product.img}" class="card-img-top">
                <form data-pid="${product.id}" class="add-item-form card-body">
                    <h5 class="card-title">
                        ${product.title}
                    </h5>
                    <p class="card-text">
                        商品價格: $${product.price}
                    </p>
                    <div class="form-group">
                        <label>購買數量</label>
                        <input id="amountInput${product.id}" class="form-control" type="number" min="1" max="20" required>
                    </div>
                    <div class="form-group">
                        <button class="btn btn-primary" type="submit">
                            <i class="fas fa-cart-plus"></i> 
                            加入購物車
                        </button>
                    </div>
                </form>
            </div>
        </div>
    `;
    // 回傳 cardElement 給使用此函數的變數
    return cardElement;
}

// 渲染商品列表至畫面上
renderProductList();

// 購物車建構式(物件導向)
// 建構式是一種「用來產生物件」的函數
function Cart() {
    // 建構式裡的 this 用來代表透過此建構式產生的物件(cart)
    // localStorage key
    // 物件.屬性 = 值
    this.key = 'example-cart';
    // 購物車的品項
    this.itemList = [];
    // TODO: 初始化購物車
    this.initCart = function () {
        //取回字串化後的物件null
        //如果該值不存在，將會回傳
        const itemListStr=localStorage.getItem(this.key);
        //轉回物件
        const defaultList=JSON.parse(itemListStr);
        console.log("[defaultList]",defaultList);
        //對於初次來到網頁的人 defaultList 會是空值 
        // if(defaultList){
        //     this.itemList=defaultList;
        // }else
        //     this.itemList=[];
        this.itemList=defaultList || [];
        this.render();
    }
    // TODO: 傳入商品id與數量並新增商品至購物車
    this.addItem = function (pid, amount) {
        console.log(pid, amount);
        // 取得商品的詳細資料
        // 陣列.find()
        const product = productList.find(product => {
            // 找到產品的id == pid 的資料
            return product.id == pid;
        });
        console.log("[商品詳情]", product);

        // TODO: 建構一個購物車品項資料
        // { title: 品名, price: 單價, amount: 數量, createdAt: 新增時間 }
        const item={
            //js語法糖
            //id:product.id
            //img:product.img,
            //price:product.price,
            // title:product.title,
            // price:product.price,
            //上面五行可以 ...product 取代 
            ...product,
            // amount:amount, 屬性跟變數名稱一樣可直接使用amount,
            amount,
            createdAt: new Date().getTime()
        };
        console.log("購物車品項",item);
        //把item放到這個購物車的itemList[]內
        //陣列.push(新資料)
        this.itemList.push(item);
        //渲染購物車清單
        this.render();

    }
    // TODO: 至購物車刪除於購物車內指定索引商品
    this.deleteItem = function (i) {
        //從清單內移除1筆索引是i的品項
        this.itemList.splice(i,1);
        //渲染出來
        this.render();
    }
    // TODO: 清空購物車
    this.emptyCart = function () {
        //清空itemList
        //[{},{},{},...]=>[]
        this.itemList = [];
        //渲染資料
        this.render();
    }
    // TODO: 更新資料到localStorage
    this.updateDataToStorage = function () {
        //取得字串化的itemList
        const itemListStr=JSON.stringify(this.itemList);
        localStorage.setItem(this.key,itemListStr);
    }
    // TODO: 渲染購物車
    this.render = function () {
        //更新資料到localStorage
        this.updateDataToStorage();
            
        
        // 選到id是cartTableBody的元素
        const $tbody = $('#cartTableBody');
        // 選到id是cartTableFoot的元素
        const $tfoot = $('#cartTableFoot');
        // 預設tbody內的內容是空值
        //清空$tbody 避免值重複顯示
        $tbody.empty();
        //預設購物車總金額為0
        let cartValue=0;
        // TODO: 將目前購物車的項目逐項取出
        //陣列.forEach((迭代子,索引)=>{})
        this.itemList.forEach((item,idx)=>{
            //console.log("[item]",item);
            //定一一個品項總價值
            const itemValue=item.price*item.amount;
            cartValue+=itemValue;
            //創建時間
            const time =moment(item.createdAt).format("YYYY/MM/DD HH:mm:ss");
            //描述一個表格的橫排
            //&times 叉叉按鈕
            const tr =`<tr>
                <td>
                    <div class="d-flex">
                        <button data-index="${idx}" class="delete-btn btn btn-danger btn-sm ">
                          &times; 
                        </button>  
                        <div>
                            <p class="m-0">${item.title}</p>
                            <p class="m-0">${time}</p>
                        </div>    
                    </div>
                </td>
                <td class="text-right">$ ${item.price}</td> 
                <td class="text-right"> ${item.amount}</td> 
                <td class="text-right">$ ${itemValue}</td>  
            <tr>`;
            //將tr放到畫面紹的$tbody內
            $tbody.append(tr);
        })
        $("#cartNumber").text(this.itemList.length);
        // 將內容渲染至tfoot內
        //DOM.innerHTML="<tr>...</tr>"
        $tfoot.html(`<tr>
                        <th>總金額</th>
                        <td colspan="3" class="text-right">$ ${cartValue}</td>
                    </tr>`);
    }
}

// 建立一個購物車的實例
// 物件 = new 建構式()
const cart = new Cart();
//初始化購物車
cart.initCart();
console.log("購物車", cart);

// TODO: 綁定新增商品至購物車的表單送出事件
// 選擇class="add-item-form"的元素
$(".add-item-form").submit(function (e) {
    e.preventDefault();
    // console.log("[準備新增購物車品項]");
    // 取得被 submit 的 .add-item-form
    const form = this;
    // console.dir(form);
    // 取得 data-pid 的值
    const pid = form.dataset.pid;
    // console.log("[產品id]", pid)
    // 取得該 pid 所對應的數量
    let amount = $(`#amountInput${pid}`).val();
    amount = parseInt(amount);
    // console.log("[數量]", amount);
    // 把產品id與數量傳給購物車的新增品項函數
    cart.addItem(pid, amount);
});

// TODO: 綁定清空購物車按鈕的點擊事件
$("#clearCartBtn").click(function () {
    console.log("[準備清空購物車]");
    cart.emptyCart();
});

// TODO: 綁定移除單一品項的點擊事件
// $(".delete-btn").click(function(){
//     console.log("要移除的按鈕",this);
// });
//綁定 #cartTableBody裡的動態生成元件 .delete-btn的點擊事件
$("#cartTableBody").delegate(".delete-btn","click",function(){
    console.log("要移除的按鈕",this);
    //javascript // const index =parseInt(this.dataset.index);
    //jQuery
    const index=parseInt($(this).attr("data-index"));
    console.log("index",index);
    // 將要移除的索引垂給deleteItem函數
    cart.deleteItem(index);
});
})