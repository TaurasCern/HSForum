window.onload = () => {
    insertHeader();
    if(!(localStorage.getItem(`token`) === null)){
        window.location.assign("index.html");
    }

    document.querySelector(`#register-button`)
        .addEventListener(`click`, (e) => {
            e.preventDefault();
            register();
    })
}

const register = () => {
    let registerForm = document.querySelector(`.register-form`);
    let data = getFormData(registerForm);
    
    let err = document.querySelector(`.error-container`);

    if(data.username.length < 3){
        err.style.padding = `5px`;
        err.style.backgroundColor = `rgb(176, 78, 78)`;
        err.style.border = `1px solid red`;
        err.innerHTML = `Username has to be from 3 to 16 characters`;
    }
    else if (data.password.length < 7){
        err.style.padding = `5px`;
        err.style.backgroundColor = `rgb(176, 78, 78)`;
        err.style.border = `1px solid red`;
        err.innerHTML = `Password has to be from 7 to 20 characters`;
    }
    else registerFetch(data.username, data.email, data.password);
}
const getFormData = (form) => {
    let formData = new FormData(form);
    let data = {};

    formData.forEach((value, key) => data[key] = value);
    
    return data;
}

const registerFetch = async (username, email, password) => {
    let response = await fetch(`http://localhost:5084/api/Register`, {
        method: `post`,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: username,
            email: email,
            password: password
        })
    })

    if(response.ok){
        document.location.assign(`index.html`);
    }
    else console.log(response.status);
}       