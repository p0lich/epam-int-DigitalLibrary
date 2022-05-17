function authorSelect(value, index) {
    if (value == '') {
        document.getElementById('Authors_' + index + '__FirstName').value = '';
        document.getElementById('Authors_' + index + '__LastName').value = '';
    }

    else {
        let splittedName = value.split(/; /)

        document.getElementById('Authors_' + index + '__FirstName').value = splittedName[0];
        document.getElementById('Authors_' + index + '__LastName').value = splittedName[1];
    }
}