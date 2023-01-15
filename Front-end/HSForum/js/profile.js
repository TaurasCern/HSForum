window.onload = () => {
    insertHeader();

    loadUser();

    document.querySelector(`#logout-button`).addEventListener(`click`, (e) =>{
        localStorage.removeItem(`token`);
        localStorage.removeItem(`roles`);
        localStorage.removeItem(`id`);
        window.location.assign(`index.html`);
    })
}

const loadUser = async () => {
    let response = await fetch(`http://localhost:5084/api/User/${localStorage.getItem(`id`)}`, {
        method: `get`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
    })

    if(response.ok){
        let json = await response.json();
        document.querySelector(`.account-container`).innerHTML = `
        ${json.username}${json.createdAt}${json.reputation}    
        `;
    }
}