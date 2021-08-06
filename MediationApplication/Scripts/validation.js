function validateMantraForm() {
    var mantraName = document.getElementById("mantraname");

    if (mantraName == "" || mantraName == null) {
        console.log("empty or null value");
        return false;

    } else {
        console.log("all good!");
        return true;
    }
}


