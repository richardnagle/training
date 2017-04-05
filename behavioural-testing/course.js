function loadMarkDown() {
        var markdown = document.querySelector('link[rel="import"]').import;
        var target = document.getElementById('source');
        target.innerText = markdown.getElementById('markdown').innerText;
}

loadMarkDown();