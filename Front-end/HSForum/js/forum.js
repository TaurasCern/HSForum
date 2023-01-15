window.onload = () => {
    insertHeader();

    loadPostTypes();
}

const loadPostTypes = async () => {
    let response = await fetch(`http://localhost:5084/api/PostType`, {
        method: `get`,
        headers: {
            'Accept': 'application/json',
        }
    })

    if(response.ok){
        let json = await response.json()
        let html = `<ul class="post-type-list">`
        json.forEach(element => {
            html += `
            <li class="post-type">
                <a href="posts.html?type=${element.type}">${element.type}</a>
            </li>
        `;
        });
        document.querySelector(`.post-type-container`).innerHTML += html +`</ul>`;
    }
    else console.log(response.status);
}
const redirectToPosts = (type) => {
    window.location.assign(`posts.html?type=${type}`);
}