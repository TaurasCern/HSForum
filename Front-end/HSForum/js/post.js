window.onload = () => {
    insertHeader();
 
    let params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });

    if(params.id != null){
        loadPost(params.id);
    }
    else window.location.assign("index.html");
}

const loadPost = (id) => {
    fetchPost(id);
}
const fetchPost = async (id) => {
    let response = await fetch(`http://localhost:5084/api/Post/${id}`, {
        method: `get`,
        headers: {
            'Accept': 'application/json',
        },
    })

    if(response.ok){
        let json = await response.json();
        console.log(json);
        document.querySelector(`.post-title`).innerHTML = json.title;
        document.querySelector(`.post-content`).innerHTML = json.content;
        document.querySelector(`.post-date`).innerHTML = (new Date(json.createdAt)).toLocaleString();
    }
    else console.log(response.status);
}

const insertPost = (container, post) => {
    
    container.innerHTML += `
    <div class="post">
        <a class="post-title" href="post.html?id=${post.postId}">
            ${post.title}
        </a>
        <div class="post-date">
            ${(new Date(post.createdAt)).toLocaleString()}
        </div>
    </div>
    `;
}