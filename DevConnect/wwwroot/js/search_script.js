document.getElementById('searchBtn').addEventListener('click', function () {
    var inputText = document.querySelector('.text-input').value;
    if (document.getElementById('articles').checked) {
        fetch('/Articles/DeleteArticle', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ title: inputText })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Article deleted successfully');
                } else {
                    alert('Failed to delete article');
                }
            })
            .catch(error => console.error('Error:', error));
    }
    if (document.getElementById('questions').checked) {

    }
    if (document.getElementById('users').checked) {

    }
});