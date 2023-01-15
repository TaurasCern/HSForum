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
    else console.log(response.status);
}