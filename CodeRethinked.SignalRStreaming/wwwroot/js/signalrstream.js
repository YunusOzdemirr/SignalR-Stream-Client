// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/.
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
//.withUrl("https://localhost:5002/myhub")
//.withUrl("/streamHub")

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/streamhub")
    .build();
document.getElementById("streamButton").addEventListener("click", (event) => __awaiter(this, void 0, void 0, function* () {
    connection.start();
    try {
        connection.stream("PriceLogStream", 1000)
            .subscribe({
                next: (item) => {
                    console.log(item);
                    var li = document.createElement("li");
                    var li2 = document.createElement("li");
                    li.textContent = item;
                    li2.textContent = "--------------------------------------------------------------------------";
                    document.getElementById("messagesList").appendChild(li);
                    document.getElementById("messagesList").appendChild(li2);
                },
                complete: () => {
                    var li = document.createElement("li");
                    li.textContent = "Stream completed";
                    document.getElementById("messagesList").appendChild(li);
                },
                error: (err) => {
                    var li = document.createElement("li");
                    li.textContent = err;
                    document.getElementById("messagesList").appendChild(li);
                },
            });
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
}));
document.getElementById("streamButton2").addEventListener("click", (event) => __awaiter(this, void 0, void 0, function* () {
    connection.start();
    try {
        connection.stream("LogStreamUserId", 1000)
            .subscribe({
                next: (item) => {
                    console.log(item);
                    var li = document.createElement("li");
                    var li2 = document.createElement("li");
                    li.textContent = item;
                    li2.textContent = "--------------------------------------------------------------------------";
                    document.getElementById("messagesList").appendChild(li);
                    document.getElementById("messagesList").appendChild(li2);
                },
                complete: () => {
                    var li = document.createElement("li");
                    li.textContent = "Stream completed";
                    document.getElementById("messagesList").appendChild(li);
                },
                error: (err) => {
                    var li = document.createElement("li");
                    li.textContent = err;
                    document.getElementById("messagesList").appendChild(li);
                },
            });
    }
    catch (e) {
        console.error(e.toString());
    }
    event.preventDefault();
}));

document.getElementById("stopStream").addEventListener("click", (event) => __awaiter(this, void 0, void 0, function* () {
    connection.stop();
    document.getElementById("messagesList").innerHTML="";
    alert('stopped');

}));

// We need an async function in order to use await, but we want this code to run immediately,
// so we use an "immediately-executed async function"
(() => __awaiter(this, void 0, void 0, function* () {
    try {
        yield connection.start();
    }
    catch (e) {
        console.error(e.toString());
    }
}))();