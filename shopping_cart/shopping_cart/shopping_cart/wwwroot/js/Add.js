window.onload = function () {
    let elemlist = document.getElementsByClassName("add_button");

    for (let i = 0; i < elemlist.length; i++) {
        elemlist[i].addEventListener("click", onAdd)
    }
}

function onAdd(event) {
    let elem = event.currentTarget;
    let productId = elem.getAttribute("photo_id");
    sendAdd(productId)
}

function sendAdd(productId) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Cart/AddItemToCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    // send add choice to server
    xhr.send(JSON.stringify({
        ProductId: productId
    }));
}