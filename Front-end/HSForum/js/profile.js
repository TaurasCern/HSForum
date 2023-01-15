window.onload = () => {
    insertHeader();

    let id = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    }).id;

    loadUser(id);

    if(localStorage.getItem(`token`) != undefined 
    && localStorage.getItem(`token`) != null
    && id === localStorage.getItem(`id`)){
        document.querySelector(`.account-container`).outerHTML += `
            <a id="logout-button">Logout</a>
        `;
        document.querySelector(`#logout-button`).addEventListener(`click`, () =>{
            localStorage.removeItem(`token`);
            localStorage.removeItem(`roles`);
            localStorage.removeItem(`id`);
            window.location.assign(`index.html`);
        })
    }
}

const loadUser = async (id) => {
    let response = await fetch(`http://localhost:5084/api/User/${id}`, {
        method: `get`,
        headers: {
            'Authorization': `Bearer ${localStorage.getItem(`token`)}`
        },
    })

    if(response.ok){
        let json = await response.json();
        document.querySelector(`.account-container`).innerHTML = `
            <div class="username">${json.username}</div>
            <div class="join-date">Joined: ${(new Date(json.createdAt)).toUTCString()}</div>
            <div class="post-count">Posts: ${json.postCount}</div>
            <div class="reputation">Reputation: ${json.reputation}</div> 
        `;
    }
    else {
        window.location.assign(`index.html`)
    }
}