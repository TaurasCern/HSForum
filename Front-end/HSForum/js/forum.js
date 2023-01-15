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

        json.forEach(element => {
            document.querySelector(`.post-type-container`).innerHTML += `
            <a href="posts.html?type=${element.type}" class="post-type">
                ${element.type}
            </a>
        `;
        });
    }
    else console.log(response.status);
}