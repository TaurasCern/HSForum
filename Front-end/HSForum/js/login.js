let isErrorOpen = false;

window.onload = () => {
    insertHeader();
    if(!(localStorage.getItem(`token`) === null)){
        window.location.assign("index.html");
    }

    document.querySelector(`#login-button`)
        .addEventListener(`click`, (e) => {
            e.preventDefault();
            login();
    })
}

const login = () => {
    let loginForm = document.querySelector(`.login-form`);
    let data = getFormData(loginForm);

    loginFetch(data.username, data.password);
}
const getFormData = (form) => {
    let formData = new FormData(form);
    let data = {};

    formData.forEach((value, key) => data[key] = value);
    
    return data;
}

const loginFetch = async (username, password) => {
    let response = await fetch(`http://localhost:5084/api/Login`, {
        method: `post`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: username,
            password: password,
        })
    })

    if(response.ok){
        let json = await response.json();
        localStorage.setItem(`token`, json.token);
        localStorage.setItem(`roles`, json.roles.join(`,`));
        localStorage.setItem(`id`, json.userId);

        document.location.assign(`index.html`);
    }
    else if(response.status === 400){
        let err = document.querySelector(`.error-container`);
        err.style.padding = `5px`;
        err.style.backgroundColor = `rgb(176, 78, 78)`;
        err.style.border = `1px solid red`;
        err.innerHTML = `User does not exist or wrong password entered`;
        
    }
    else console.log(response.status);
}