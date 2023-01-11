window.onload = () => {
    insertHeader();

    document.querySelector(`#logout-button`).addEventListener(`click`, (e) =>{
        localStorage.removeItem(`token`);
        window.location.assign(`index.html`);
    })
}