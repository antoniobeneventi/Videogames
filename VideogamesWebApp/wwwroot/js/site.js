document.addEventListener("DOMContentLoaded", function () {

    let currentPage = 1;
    const pageSize = 5;

    function fetchStores(page) {
        fetch(`${getAllStoresUrl}?page=${page}&pageSize=${pageSize}`)
            .then(response => response.json())
            .then(data => {
                let storeListHtml = '';
                data.stores.forEach(store => {
                    storeListHtml += `
                                    <div class="store-item mb-3">
                                        <h5 class="store-name">${store.storeName}</h5>
                                        <p class="store-description">${store.storeDescription || 'No description available'}</p>
                                        <a href="${store.storeLink}" target="_blank" class="store-link">Visit Store</a>
                                    </div>
                                `;
                });
                document.getElementById("storeListContainer").innerHTML = storeListHtml;

                currentPage = data.currentPage;
                document.getElementById("pageInfo").innerText = `Page ${currentPage} of ${data.totalPages}`;

                let paginationHtml = '';
                for (let i = 1; i <= data.totalPages; i++) {
                    paginationHtml += `
                                    <button class="page-btn ${i === currentPage ? 'active' : ''}" data-page="${i}">
                                        ${i}
                                    </button>
                                `;
                }
                document.getElementById("paginationContainer").innerHTML = paginationHtml;

                document.querySelectorAll(".page-btn").forEach(button => {
                    button.addEventListener("click", function () {
                        const page = parseInt(this.getAttribute("data-page"));
                        fetchStores(page);
                    });
                });
            })
            .catch(error => console.error("Error fetching store data:", error));
    }

    document.getElementById("viewAllStoresBtn").addEventListener("click", function () {
        currentPage = 1;
        fetchStores(currentPage);
    });

    let currentPlatformPage = 1;
    const platformPageSize = 5;

    function fetchPlatforms(page) {
        fetch(`${getAllPlatformsUrl}?page=${page}&pageSize=${platformPageSize}`)
            .then(response => response.json())
            .then(data => {
                let platformListHtml = '';
                data.platforms.forEach(platform => {
                    platformListHtml += `
                                        <div class="platform-item mb-3">
                                            <h5 class="platform-name">${platform.platformName}</h5>
                                            <p class="platform-description">${platform.platformDescription || 'No description available'}</p>
                                        </div>
                                    `;
                });
                document.getElementById("platformListContainer").innerHTML = platformListHtml;

                document.getElementById("platformPageInfo").innerText = `Page ${data.currentPage} of ${data.totalPages}`;
                updatePaginationButtons(data.currentPage, data.totalPages);
            })
            .catch(error => console.error("Error fetching platform data:", error));
    }

    function updatePaginationButtons(currentPage, totalPages) {
        const paginationContainer = document.getElementById("platformPaginationContainer");
        paginationContainer.innerHTML = '';

        for (let i = 1; i <= totalPages; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `btn btn-secondary ${i === currentPage ? 'active' : ''}`;
            pageButton.innerText = i;
            pageButton.setAttribute('data-page', i);

            pageButton.addEventListener('click', function () {
                currentPlatformPage = i;
                fetchPlatforms(currentPlatformPage);
            });

            paginationContainer.appendChild(pageButton);
        }
    }

    document.getElementById("viewAllPlatformsBtn").addEventListener("click", function () {
        currentPlatformPage = 1;
        fetchPlatforms(currentPlatformPage);
    });

    let currentLauncherPage = 1;
    const launcherPageSize = 5;

    function fetchLaunchers(page) {
        fetch(`${getAllLaunchersUrl}?page=${page}&pageSize=${launcherPageSize}`)
            .then(response => response.json())
            .then(data => {
                let launcherListHtml = '';
                data.launchers.forEach(launcher => {
                    launcherListHtml += `
                                            <div class="launcher-item mb-3">
                                                <h5 class="launcher-name">${launcher.launcherName}</h5>
                                                <p class="launcher-description">${launcher.launcherDescription || 'No description available'}</p>
                                            </div>
                                        `;
                });
                document.getElementById("launcherListContainer").innerHTML = launcherListHtml;

                document.getElementById("launcherPageInfo").innerText = `Page ${data.currentPage} of ${data.totalPages}`;
                updateLauncherPaginationButtons(data.currentPage, data.totalPages);
            })
            .catch(error => console.error("Error fetching launcher data:", error));
    }

    function updateLauncherPaginationButtons(currentPage, totalPages) {
        const paginationContainer = document.getElementById("launcherPaginationContainer");
        paginationContainer.innerHTML = '';

        for (let i = 1; i <= totalPages; i++) {
            const pageButton = document.createElement('button');
            pageButton.className = `btn btn-secondary ${i === currentPage ? 'active' : ''}`;
            pageButton.innerText = i;
            pageButton.setAttribute('data-page', i);

            pageButton.addEventListener('click', function () {
                currentLauncherPage = i;
                fetchLaunchers(currentLauncherPage);
            });

            paginationContainer.appendChild(pageButton);
        }
    }

    document.getElementById("viewAllLaunchersBtn").addEventListener("click", function () {
        currentLauncherPage = 1;
        fetchLaunchers(currentLauncherPage);
    });
});
let fromStoreErrorButton = false;
let fromPlatformErrorButton = false;
let fromLauncherErrorButton = false;


function showError(inputElement, errorElement, isValid, customMessage = '', buttonHtml = '') {
    if (!isValid) {
        errorElement.innerHTML = `
                                                            <div class="error-message">${customMessage}</div>
                                                            ${buttonHtml.replace('btn btn-sm btn-primary mt-2', 'error-action-button')}
                                                        `;
        errorElement.style.display = 'block';
        inputElement.classList.add('is-invalid');
    } else {
        errorElement.style.display = 'none';
        inputElement.classList.remove('is-invalid');
    }
}
function validateForm() {
    let isValid = true;

    // Validate Game
    if (!gameSelected.value) {
        const gameButton = `<button type="button" class="btn btn-sm btn-primary mt-2" onclick="redirectToAddGame()">Add New Game</button>`;
        showError(gameSearch, document.getElementById('gameSearchError'), false, "Please enter a valid game or add it if you want", gameButton);
        isValid = false;
    }

    // Validate Store
    if (!storeSelected.value) {
        const storeButton = `<button type="button" class="btn btn-sm btn-primary mt-2" onclick="openAddStoreModal('${storeSearch.value}')">Add New Store</button>`;
        showError(storeSearch, document.getElementById('storeSearchError'), false, "Please enter a valid store or add it if you want", storeButton);
        isValid = false;
    }

    // Validate Platform
    if (!platformSelected.value) {
        const platformButton = '<button type="button" class="btn btn-sm btn-primary mt-2" onclick="openAddPlatformModal()">Add New Platform</button>';
        showError(platformSearch, document.getElementById('platformSearchError'), false, "Please enter a valid platform or add it if you want", platformButton);
        isValid = false;
    }

    // Validate Launcher
    if (!launcherSelected.value) {
        const launcherButton = '<button type="button" class="btn btn-sm btn-primary mt-2" onclick="openAddLauncherModal()">Add New Launcher</button>';
        showError(launcherSearch, document.getElementById('launcherSearchError'), false, "Please enter a valid launcher or add it if you want", launcherButton);
        isValid = false;
    }

    return isValid;
}

function redirectToAddGame() {
    const gameName = gameSearch.value;
    const payload = {
        storeName: storeSearch.value,
        platformName: platformSearch.value,
        launcherName: launcherSearch.value
        
    };

    sessionStorage.setItem("payload", JSON.stringify(payload));

    window.location.href = `ViewAllGames?gameName=${encodeURIComponent(gameName)}`;
}
document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    const gameName = urlParams.get('gameName');

    if (gameName) {
        const buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
        const payload = JSON.parse(sessionStorage.getItem("payload"));
        if (payload) {
            document.getElementById('storeSearch').value = payload.storeName; 
            document.getElementById('platformSearch').value = payload.platformName; 

            document.getElementById('launcherSearch').value = payload.launcherName; 

            sessionStorage.removeItem("payload");
        }
        document.getElementById('gameSearch').value = gameName; 
        buyGameModal.show();
    }

    // Clear inputs when the modal is closed
    document.getElementById('buyGameModal').addEventListener('hidden.bs.modal', function (e) {
        document.getElementById('gameSearch').value = '';
        document.getElementById('storeSearch').value = '';
        document.getElementById('platformSearch').value = '';
        document.getElementById('launcherSearch').value = '';
    });
});


function openAddStoreModal(storeName) {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide();

    fromStoreErrorButton = true;

    document.getElementById('storeName').value = storeName;

    let storeModal = new bootstrap.Modal(document.getElementById('addStoreModal'));
    storeModal.show();
}


function openAddPlatformModal() {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide();

    document.getElementById('platformName').value = platformSearch.value;

    fromPlatformErrorButton = true;

    let platformModal = new bootstrap.Modal(document.getElementById('addPlatformModal'));
    platformModal.show();
}

function openAddLauncherModal() {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide();

    document.getElementById('launcherName').value = launcherSearch.value;


    fromLauncherErrorButton = true;

    let launcherModal = new bootstrap.Modal(document.getElementById('addLauncherModal'));
    launcherModal.show();
}

function handleFormSubmit(formId, url, updateFunction, closeModal = true) {
    document.getElementById(formId).addEventListener('submit', function (e) {
        e.preventDefault();
        const formData = new FormData(this);

        fetch(url, {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    if (formId === 'addStoreForm') {
                        let addStoreModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addStoreModal.hide();

                        document.getElementById(formId).reset();

                        if (fromStoreErrorButton) {
                            openBuyGameModalWithStore(data.storeName, data.storeId);
                            fromStoreErrorButton = false;
                        }
                    } else if (formId === 'addPlatformForm') {
                        let addPlatformModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addPlatformModal.hide();

                        document.getElementById(formId).reset();

                        if (fromPlatformErrorButton) {
                            openBuyGameModalWithPlatform(data.platformName, data.platformId);
                            fromPlatformErrorButton = false;
                        }
                    } else if (formId === 'addLauncherForm') {
                        let addLauncherModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addLauncherModal.hide();

                        document.getElementById(formId).reset();

                        if (fromLauncherErrorButton) {
                            openBuyGameModalWithLauncher(data.launcherName, data.launcherId);
                            fromLauncherErrorButton = false;
                        }
                    } else {
                        updateFunction(data);
                        if (closeModal) {
                            bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal')).hide();
                        }
                    }
                } else {
                    alert(data.message);
                }
            })
            .catch(error => console.error('Error:', error));
    });
}


function openBuyGameModalWithStore(storeName, storeId) {

    const errorMessageElement = document.getElementById('storeSearchError');
    errorMessageElement.style.display = 'none';
    errorMessageElement.innerHTML = '';


    document.getElementById('storeSearch').value = storeName;
    document.getElementById('storeId').value = storeId;

    storeSelected.value = true;

    document.getElementById('storeSearch').classList.remove('is-invalid');


    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show();
}


function openBuyGameModalWithPlatform(platformName, platformId) {

    const errorMessageElement = document.getElementById('platformSearchError');
    errorMessageElement.style.display = 'none';
    errorMessageElement.innerHTML = '';


    document.getElementById('platformSearch').value = platformName;
    document.getElementById('platformId').value = platformId;

    platformSelected.value = true;
    document.getElementById('platformSearch').classList.remove('is-invalid');

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show();
}

function openBuyGameModalWithLauncher(launcherName, launcherId) {

    const errorMessageElement = document.getElementById('launcherSearchError');
    errorMessageElement.style.display = 'none';
    errorMessageElement.innerHTML = '';


    document.getElementById('launcherSearch').value = launcherName;
    document.getElementById('launcherId').value = launcherId;

    launcherSelected.value = true;
    document.getElementById('launcherSearch').classList.remove('is-invalid');

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show();
}

// Funzioni per aggiornare i campi nel modale di acquisto
function updateStoreField(data) {
    document.getElementById('storeSearch').value = data.storeName;
    document.getElementById('storeId').value = data.storeId;
    storeSelected.value = true;
    showError(storeSearch, document.getElementById('storeSearchError'), true);
}

function updatePlatformField(data) {
    document.getElementById('platformSearch').value = data.platformName;
    document.getElementById('platformId').value = data.platformId;
    platformSelected.value = true;
    showError(platformSearch, document.getElementById('platformSearchError'), true);

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show();
}

function updateLauncherField(data) {
    document.getElementById('launcherSearch').value = data.launcherName;
    document.getElementById('launcherId').value = data.launcherId;
    launcherSelected.value = true;
    showError(launcherSearch, document.getElementById('launcherSearchError'), true);

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show();
}

document.addEventListener('DOMContentLoaded', function () {
    handleFormSubmit('addStoreForm', '/Games/AddStore', updateStoreField);
    handleFormSubmit('addPlatformForm', '/Games/AddPlatform', updatePlatformField);
    handleFormSubmit('addLauncherForm', '/Games/AddLauncher', updateLauncherField);

});

document.getElementById('buyGameModal').addEventListener('hidden.bs.modal', function (e) {

    if (!document.querySelector('.modal.show')) {
        gameSearch.value = '';
        storeSearch.value = '';
        platformSearch.value = '';
        launcherSearch.value = '';

        gameId.value = '';
        storeId.value = '';
        platformId.value = '';
        launcherId.value = '';
    }
});


document.getElementById('buyGameForm').addEventListener('submit', function (event) {
    if (!validateForm()) {
        event.preventDefault(); // Previene l'invio del form se non è valido
    }
});

function selectSuggestion(inputElement, hiddenInputElement, selectedItem, flagRef, errorElement) {
    inputElement.value = selectedItem.textContent;
    hiddenInputElement.value = selectedItem.dataset.id;
    flagRef.value = true;
    showError(inputElement, errorElement, true);
    document.querySelectorAll('.search-results').forEach(results => {
        results.style.display = 'none';
    });
}

const gameSelected = { value: false };
gameSearch.addEventListener('input', function () {
    gameSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchGames?query=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(games => {
                gameSearchResults.innerHTML = '';
                if (games.length > 0) {
                    games.forEach(game => {
                        const div = document.createElement('div');
                        div.className = 'search-result-item';
                        div.textContent = game.gameName;
                        div.dataset.id = game.gameId;


                        div.addEventListener('mousedown', () => {
                            selectSuggestion(gameSearch, gameId, div, gameSelected, document.getElementById('gameSearchError'));
                        });

                        gameSearchResults.appendChild(div);
                    });
                    gameSearchResults.style.display = 'block';
                } else {
                    gameSearchResults.style.display = 'none';
                }
            });
    } else {
        gameSearchResults.style.display = 'none';
    }
});

// Store Search
const storeSelected = { value: false };
storeSearch.addEventListener('input', function () {
    storeSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchStores?query=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(stores => {
                storeSearchResults.innerHTML = '';
                if (stores.length > 0) {
                    stores.forEach(store => {
                        const div = document.createElement('div');
                        div.className = 'search-result-item';
                        div.textContent = store.storeName;
                        div.dataset.id = store.storeId;
                        div.addEventListener('mousedown', () => {
                            selectSuggestion(storeSearch, storeId, div, storeSelected, document.getElementById('storeSearchError'));
                        });
                        storeSearchResults.appendChild(div);
                    });
                    storeSearchResults.style.display = 'block';
                } else {
                    storeSearchResults.style.display = 'none';
                }
            });
    } else {
        storeSearchResults.style.display = 'none';
    }
});
// Platform Search
const platformSelected = { value: false };
platformSearch.addEventListener('input', function () {
    platformSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchPlatforms?query=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(platforms => {
                platformSearchResults.innerHTML = '';
                if (platforms.length > 0) {
                    platforms.forEach(platform => {
                        const div = document.createElement('div');
                        div.className = 'search-result-item';
                        div.textContent = platform.platformName;
                        div.dataset.id = platform.platformId;
                        div.addEventListener('mousedown', () => {
                            selectSuggestion(platformSearch, platformId, div, platformSelected, document.getElementById('platformSearchError'));
                        });
                        platformSearchResults.appendChild(div);
                    });
                    platformSearchResults.style.display = 'block';
                } else {
                    platformSearchResults.style.display = 'none';
                }
            });
    } else {
        platformSearchResults.style.display = 'none';
    }
});

// Launcher Search
const launcherSelected = { value: false };
launcherSearch.addEventListener('input', function () {
    launcherSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchLaunchers?query=${encodeURIComponent(searchQuery)}`)
            .then(response => response.json())
            .then(launchers => {
                launcherSearchResults.innerHTML = '';
                if (launchers.length > 0) {
                    launchers.forEach(launcher => {
                        const div = document.createElement('div');
                        div.className = 'search-result-item';
                        div.textContent = launcher.launcherName;
                        div.dataset.id = launcher.launcherId;
                        div.addEventListener('mousedown', () => {
                            selectSuggestion(launcherSearch, launcherId, div, launcherSelected, document.getElementById('launcherSearchError'));
                        });
                        launcherSearchResults.appendChild(div);
                    });
                    launcherSearchResults.style.display = 'block';
                } else {
                    launcherSearchResults.style.display = 'none';
                }
            });
    } else {
        launcherSearchResults.style.display = 'none';
    }
});

document.addEventListener('click', function (e) {
    if (!e.target.closest('.search-container')) {
        document.querySelectorAll('.search-results').forEach(results => {
            results.style.display = 'none';
        });
    }
});



document.addEventListener("DOMContentLoaded", function () {

    const successMessage = document.querySelector(".alert-success");
    if (successMessage) {
        setTimeout(() => {
            successMessage.style.display = "none";
        }, 3000);
    }

    const errorMessage = document.querySelector(".alert-danger");
    if (errorMessage) {
        setTimeout(() => {
            errorMessage.style.display = "none";
        }, 3000);
    }
});


document.getElementById('storeSearch').addEventListener('input', function () {
    const inputValue = this.value;
    validateStoreInput(inputValue);
});

function validateStoreInput(inputValue) {
    const errorMessageElement = document.getElementById('storeSearchError');
    const storeInput = document.getElementById('storeSearch');

    if (existingStores.includes(inputValue)) {
        errorMessageElement.style.display = 'none';
        storeInput.classList.remove('is-invalid');
    } else {
        errorMessageElement.style.display = 'block';
        errorMessageElement.innerHTML = 'Store not found. Please select from the dropdown or create a new store.';
        storeInput.classList.add('is-invalid');
    }
}
//function redirectToAddGame(url) {
//    window.location.href = url;
//}

function showDropdownSuggestions() {
    const storeSearchResults = document.getElementById('storeSearchResults');
    storeSearchResults.style.display = 'block';
}

document.getElementById('storeSearch').addEventListener('input', function () {
    showDropdownSuggestions();
});

document.addEventListener("DOMContentLoaded", function () {
    const purchaseDateInput = document.getElementById("purchaseDate");
    const today = new Date().toISOString().split("T")[0];
    purchaseDateInput.value = today;
});







