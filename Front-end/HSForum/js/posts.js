window.onload = () => {
    insertHeader();
 
    let params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });

    if(params.type != null){
        loadPosts(params.type);
    }
    else window.location.assign("index.html");
}

const loadPosts = (type) => {
    fetchPosts(type);
}
const fetchPosts = async (type) => {
    let response = await fetch(`http://localhost:5084/api/Post/${type}`, {
        method: `get`,
        headers: {
            'Accept': 'application/json',
        },
    })

    if(response.ok){
        let json = await response.json();
        if(json.length != 0){
            json.forEach(element => { 
                insertPost(document.querySelector(`.posts-container`), element) 
            });
        }
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