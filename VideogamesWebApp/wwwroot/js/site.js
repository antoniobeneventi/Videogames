document.addEventListener("DOMContentLoaded", function () {

    let currentPage = 1; // Pagina corrente per le liste di store, piattaforme e launcher
    const pageSize = 5; // Numero di elementi per pagina

    // Funzione per ottenere e visualizzare la lista degli store
    function fetchStores(page) {
        fetch(`${getAllStoresUrl}?page=${page}&pageSize=${pageSize}`)
            .then(response => response.json())
            .then(data => {
                let storeListHtml = '';
                // Costruzione della lista degli store
                data.stores.forEach(store => {
                    storeListHtml += `
                                    <div class="store-item mb-3">
                                        <h5 class="store-name">${store.storeName}</h5>
                                        <p class="store-description">${store.storeDescription || 'No description available'}</p>
                                        <a href="${store.storeLink}" target="_blank" class="store-link">Visit Store</a>
                                    </div>
                                `;
                });
                document.getElementById("storeListContainer").innerHTML = storeListHtml; // Aggiorna la lista di store

                currentPage = data.currentPage; // Imposta la pagina corrente
                document.getElementById("pageInfo").innerText = `Page ${currentPage} of ${data.totalPages}`; // Aggiorna le informazioni della pagina

                let paginationHtml = '';
                // Crea i pulsanti per la paginazione
                for (let i = 1; i <= data.totalPages; i++) {
                    paginationHtml += `
                                    <button class="page-btn ${i === currentPage ? 'active' : ''}" data-page="${i}">
                                        ${i}
                                    </button>
                                `;
                }
                document.getElementById("paginationContainer").innerHTML = paginationHtml;

                // Aggiungi un listener per ogni pulsante di paginazione
                document.querySelectorAll(".page-btn").forEach(button => {
                    button.addEventListener("click", function () {
                        const page = parseInt(this.getAttribute("data-page"));
                        fetchStores(page); // Carica la pagina selezionata
                    });
                });
            })
            .catch(error => console.error("Error fetching store data:", error)); // Gestione errori
    }

    // Mostra tutti gli store al clic del pulsante
    document.getElementById("viewAllStoresBtn").addEventListener("click", function () {
        currentPage = 1;
        fetchStores(currentPage); // Carica la prima pagina
    });

    let currentPlatformPage = 1; // Pagina corrente per la lista delle piattaforme
    const platformPageSize = 5; // Numero di piattaforme per pagina

    // Funzione per ottenere e visualizzare la lista delle piattaforme
    function fetchPlatforms(page) {
        fetch(`${getAllPlatformsUrl}?page=${page}&pageSize=${platformPageSize}`)
            .then(response => response.json())
            .then(data => {
                let platformListHtml = '';
                // Costruzione della lista delle piattaforme
                data.platforms.forEach(platform => {
                    platformListHtml += `
                                        <div class="platform-item mb-3">
                                            <h5 class="platform-name">${platform.platformName}</h5>
                                            <p class="platform-description">${platform.platformDescription || 'No description available'}</p>
                                        </div>
                                    `;
                });
                document.getElementById("platformListContainer").innerHTML = platformListHtml; // Aggiorna la lista delle piattaforme

                document.getElementById("platformPageInfo").innerText = `Page ${data.currentPage} of ${data.totalPages}`; // Mostra informazioni sulla pagina
                updatePaginationButtons(data.currentPage, data.totalPages); // Aggiorna la paginazione
            })
            .catch(error => console.error("Error fetching platform data:", error)); // Gestione errori
    }

    // Funzione per aggiornare i pulsanti di paginazione delle piattaforme
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
                fetchPlatforms(currentPlatformPage); // Carica la pagina selezionata
            });

            paginationContainer.appendChild(pageButton); // Aggiungi il pulsante alla paginazione
        }
    }

    // Mostra tutte le piattaforme al clic del pulsante
    document.getElementById("viewAllPlatformsBtn").addEventListener("click", function () {
        currentPlatformPage = 1;
        fetchPlatforms(currentPlatformPage); // Carica la prima pagina
    });

    let currentLauncherPage = 1; // Pagina corrente per la lista dei launcher
    const launcherPageSize = 5; // Numero di launcher per pagina

    // Funzione per ottenere e visualizzare la lista dei launcher
    function fetchLaunchers(page) {
        fetch(`${getAllLaunchersUrl}?page=${page}&pageSize=${launcherPageSize}`)
            .then(response => response.json())
            .then(data => {
                let launcherListHtml = '';
                // Costruzione della lista dei launcher
                data.launchers.forEach(launcher => {
                    launcherListHtml += `
                                            <div class="launcher-item mb-3">
                                                <h5 class="launcher-name">${launcher.launcherName}</h5>
                                                <p class="launcher-description">${launcher.launcherDescription || 'No description available'}</p>
                                            </div>
                                        `;
                });
                document.getElementById("launcherListContainer").innerHTML = launcherListHtml; // Aggiorna la lista dei launcher

                document.getElementById("launcherPageInfo").innerText = `Page ${data.currentPage} of ${data.totalPages}`; // Mostra informazioni sulla pagina
                updateLauncherPaginationButtons(data.currentPage, data.totalPages); // Aggiorna la paginazione dei launcher
            })
            .catch(error => console.error("Error fetching launcher data:", error)); // Gestione errori
    }

    // Funzione per aggiornare i pulsanti di paginazione dei launcher
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
                fetchLaunchers(currentLauncherPage); // Carica la pagina selezionata
            });

            paginationContainer.appendChild(pageButton); // Aggiungi il pulsante alla paginazione
        }
    }

    // Mostra tutti i launcher al clic del pulsante
    document.getElementById("viewAllLaunchersBtn").addEventListener("click", function () {
        currentLauncherPage = 1;
        fetchLaunchers(currentLauncherPage); // Carica la prima pagina
    });
});

// Funzione per mostrare un messaggio di errore su un campo di input
let fromStoreErrorButton = false; // Flag per errore da store
let fromPlatformErrorButton = false; // Flag per errore da piattaforma
let fromLauncherErrorButton = false; // Flag per errore da launcher
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

// Funzione per validare il modulo
function validateForm() {
    let isValid = true;

    // Validazione del gioco
    if (!gameSelected.value) {
        const gameButton = `<button type="button" class="btn btn-sm btn-primary mt-2" onclick="redirectToAddGame()">Add New Game</button>`;
        showError(gameSearch, document.getElementById('gameSearchError'), false, "Please enter a valid game or add it if you want", gameButton);
        isValid = false;
    }

    // Validazione dello store
    if (!storeSelected.value) {
        const storeButton = `<button type="button" class="btn btn-sm btn-primary mt-2" onclick="openAddStoreModal('${storeSearch.value}')">Add New Store</button>`;
        showError(storeSearch, document.getElementById('storeSearchError'), false, "Please enter a valid store or add it if you want", storeButton);
        isValid = false;
    }

    // Validazione della piattaforma
    if (!platformSelected.value) {
        const platformButton = '<button type="button" class="btn btn-sm btn-primary mt-2" onclick="redirectToAddPlatform()">Add New Platform</button>';
        showError(platformSearch, document.getElementById('platformSearchError'), false, "Please enter a valid platform or add it if you want", platformButton);
        isValid = false;
    }

    // Validazione del launcher
    if (!launcherSelected.value) {
        const launcherButton = '<button type="button" class="btn btn-sm btn-primary mt-2" onclick="redirectToAddLauncher()">Add New Launcher</button>';
        showError(launcherSearch, document.getElementById('launcherSearchError'), false, "Please enter a valid launcher or add it if you want", launcherButton);
        isValid = false;
    }

    return isValid;
}


// Funzione per reindirizzare alla pagina di "View All Games" con il nome del gioco come parametro nella URL
function redirectToAddGame() {
    const gameName = gameSearch.value;
    const price = document.getElementById('price').value; // Get the price input
    const purchaseDate = document.getElementById('purchaseDate').value; // Get the purchase date input
    const notes = document.getElementById('notes').value; // Get the notes input

    const payload = {
        storeName: storeSearch.value,
        platformName: platformSearch.value,
        launcherName: launcherSearch.value,
        price: price,
        purchaseDate: purchaseDate,
        notes: notes
    };

    sessionStorage.setItem("payload", JSON.stringify(payload));

    window.location.href = `ViewAllGames?gameName=${encodeURIComponent(gameName)}&price=${encodeURIComponent(price)}&purchaseDate=${encodeURIComponent(purchaseDate)}&notes=${encodeURIComponent(notes)}`;
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
            document.getElementById('price').value = payload.price;
            document.getElementById('purchaseDate').value = payload.purchaseDate;
            document.getElementById('notes').value = payload.notes;


            sessionStorage.removeItem("payload");
        }
        document.getElementById('gameSearch').value = gameName;
        buyGameModal.show();
    }

    //  svuota i campi di input di 'buyGameModal'
    document.getElementById('buyGameModal').addEventListener('hidden.bs.modal', function (e) {
        document.getElementById('gameSearch').value = '';
        document.getElementById('storeSearch').value = '';
        document.getElementById('platformSearch').value = '';
        document.getElementById('launcherSearch').value = '';
    });
});

// Funzione per aprire il modal per aggiungere un nuovo store
function openAddStoreModal(storeName) {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide(); // Nascondi il modal corrente

    fromStoreErrorButton = true; // Imposta il flag per l'errore dallo store

    document.getElementById('storeName').value = storeName; // Precompila il campo store con il nome passato

    let storeModal = new bootstrap.Modal(document.getElementById('addStoreModal'));
    storeModal.show(); // Mostra il modal per aggiungere un nuovo store
}

// Funzione per aprire il modal per aggiungere una nuova piattaforma
function openAddPlatformModal() {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide(); // Nascondi il modal corrente

    document.getElementById('platformName').value = platformSearch.value; // Precompila con il valore della piattaforma

    fromPlatformErrorButton = true; // Imposta il flag per l'errore dalla piattaforma

    let platformModal = new bootstrap.Modal(document.getElementById('addPlatformModal'));
    platformModal.show(); // Mostra il modal per aggiungere una piattaforma
}

// Funzione per aprire il modal per aggiungere un nuovo launcher
function openAddLauncherModal() {
    let buyGameModal = bootstrap.Modal.getInstance(document.getElementById('buyGameModal'));
    buyGameModal.hide(); // Nascondi il modal corrente

    document.getElementById('launcherName').value = launcherSearch.value; // Precompila il campo con il nome del launcher

    fromLauncherErrorButton = true; // Imposta il flag per l'errore dal launcher

    let launcherModal = new bootstrap.Modal(document.getElementById('addLauncherModal'));
    launcherModal.show(); // Mostra il modal per aggiungere un launcher
}

// Funzione per gestire l'invio del modulo
function handleFormSubmit(formId, url, updateFunction, closeModal = true) {
    document.getElementById(formId).addEventListener('submit', function (e) {
        e.preventDefault(); // Impedisce l'invio predefinito del modulo
        const formData = new FormData(this); // Ottieni i dati del modulo

        fetch(url, {
            method: 'POST',
            body: formData // Invia i dati del modulo tramite POST
        })
            .then(response => response.json()) // Converte la risposta in formato JSON
            .then(data => {
                if (data.success) { // Se la risposta è positiva
                    if (formId === 'addStoreForm') {
                        let addStoreModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addStoreModal.hide(); // Nascondi il modal dopo l'invio del modulo

                        document.getElementById(formId).reset(); // Reset dei campi del modulo

                        if (fromStoreErrorButton) {
                            openBuyGameModalWithStore(data.storeName, data.storeId); // Apre il modal per comprare un gioco con i dati dello store
                            fromStoreErrorButton = false; // Resetta il flag di errore
                        }
                    } else if (formId === 'addPlatformForm') {
                        let addPlatformModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addPlatformModal.hide(); // Nascondi il modal dopo l'invio del modulo

                        document.getElementById(formId).reset(); // Reset dei campi del modulo

                        if (fromPlatformErrorButton) {
                            openBuyGameModalWithPlatform(data.platformName, data.platformId); // Apre il modal per comprare un gioco con i dati della piattaforma
                            fromPlatformErrorButton = false; // Resetta il flag di errore
                        }
                    } else if (formId === 'addLauncherForm') {
                        let addLauncherModal = bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal'));
                        addLauncherModal.hide(); // Nascondi il modal dopo l'invio del modulo

                        document.getElementById(formId).reset(); // Reset dei campi del modulo

                        if (fromLauncherErrorButton) {
                            openBuyGameModalWithLauncher(data.launcherName, data.launcherId); // Apre il modal per comprare un gioco con i dati del launcher
                            fromLauncherErrorButton = false; // Resetta il flag di errore
                        }
                    } else {
                        updateFunction(data); // Se la risposta riguarda un altro modulo, esegui la funzione di aggiornamento
                        if (closeModal) {
                            bootstrap.Modal.getInstance(document.getElementById(formId).closest('.modal')).hide(); // Nascondi il modal dopo l'aggiornamento
                        }
                    }
                } else {
                    alert(data.message); // Mostra un messaggio di errore se la risposta è negativa
                }
            })
            .catch(error => console.error('Error:', error)); // Gestione errori
    });
}

// Funzione per aprire il modal per acquistare un gioco con i dati dello store
function openBuyGameModalWithStore(storeName, storeId) {
    const errorMessageElement = document.getElementById('storeSearchError');
    errorMessageElement.style.display = 'none'; // Nascondi eventuali errori

    document.getElementById('storeSearch').value = storeName; // Precompila il campo store con il nome
    document.getElementById('storeId').value = storeId; // Precompila con l'ID dello store

    storeSelected.value = true; // Imposta il valore selezionato dello store
    document.getElementById('storeSearch').classList.remove('is-invalid'); // Rimuove l'indicatore di errore

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show(); // Mostra il modal per acquistare il gioco
}

// Funzione per aprire il modal per acquistare un gioco con i dati della piattaforma
function openBuyGameModalWithPlatform(platformName, platformId) {
    const errorMessageElement = document.getElementById('platformSearchError');
    errorMessageElement.style.display = 'none'; // Nascondi eventuali errori

    document.getElementById('platformSearch').value = platformName; // Precompila il campo piattaforma con il nome
    document.getElementById('platformId').value = platformId; // Precompila con l'ID della piattaforma

    platformSelected.value = true; // Imposta il valore selezionato della piattaforma
    document.getElementById('platformSearch').classList.remove('is-invalid'); // Rimuove l'indicatore di errore

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show(); // Mostra il modal per acquistare il gioco
}


// Funzione per aprire il modal per acquistare un gioco con i dati del launcher
function openBuyGameModalWithLauncher(launcherName, launcherId) {

    const errorMessageElement = document.getElementById('launcherSearchError');
    errorMessageElement.style.display = 'none'; // Nasconde eventuali messaggi di errore
    errorMessageElement.innerHTML = ''; // Svuota il messaggio di errore

    document.getElementById('launcherSearch').value = launcherName; // Precompila il campo con il nome del launcher
    document.getElementById('launcherId').value = launcherId; // Precompila con l'ID del launcher

    launcherSelected.value = true; // Imposta il flag che indica che un launcher è stato selezionato
    document.getElementById('launcherSearch').classList.remove('is-invalid'); // Rimuove la classe di errore, se presente

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show(); // Mostra il modal per acquistare un gioco
}

// Funzioni per aggiornare i campi nel modale di acquisto
function updateStoreField(data) {
    document.getElementById('storeSearch').value = data.storeName; // Precompila il campo con il nome dello store
    document.getElementById('storeId').value = data.storeId; // Precompila con l'ID dello store
    storeSelected.value = true; // Imposta il flag per indicare che lo store è selezionato
    showError(storeSearch, document.getElementById('storeSearchError'), true); // Mostra eventuali errori
}

function updatePlatformField(data) {
    document.getElementById('platformSearch').value = data.platformName; // Precompila il campo con il nome della piattaforma
    document.getElementById('platformId').value = data.platformId; // Precompila con l'ID della piattaforma
    platformSelected.value = true; // Imposta il flag per la piattaforma selezionata
    showError(platformSearch, document.getElementById('platformSearchError'), true); // Mostra eventuali errori

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show(); // Mostra il modal per acquistare un gioco
}

function updateLauncherField(data) {
    document.getElementById('launcherSearch').value = data.launcherName; // Precompila il campo con il nome del launcher
    document.getElementById('launcherId').value = data.launcherId; // Precompila con l'ID del launcher
    launcherSelected.value = true; // Imposta il flag per il launcher selezionato
    showError(launcherSearch, document.getElementById('launcherSearchError'), true); // Mostra eventuali errori

    let buyGameModal = new bootstrap.Modal(document.getElementById('buyGameModal'));
    buyGameModal.show(); // Mostra il modal per acquistare un gioco
}


document.addEventListener('DOMContentLoaded', function () {
    handleFormSubmit('addStoreForm', '/Games/AddStore', updateStoreField); // Gestisce l'invio del modulo per aggiungere uno store
    handleFormSubmit('addPlatformForm', '/Games/AddPlatform', updatePlatformField); // Gestisce l'invio del modulo per aggiungere una piattaforma
    handleFormSubmit('addLauncherForm', '/Games/AddLauncher', updateLauncherField); // Gestisce l'invio del modulo per aggiungere un launcher
});

// Gestisce la chiusura del modal e resetta i campi
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

// Validazione del modulo di acquisto prima dell'invio
document.getElementById('buyGameForm').addEventListener('submit', function (event) {
    if (!validateForm()) {
        event.preventDefault(); // Previene l'invio del form se non è valido
    }
});

// Seleziona un elemento dalla lista delle suggerimenti
function selectSuggestion(inputElement, hiddenInputElement, selectedItem, flagRef, errorElement) {
    inputElement.value = selectedItem.textContent; // Imposta il valore dell'input con il nome dell'elemento selezionato
    hiddenInputElement.value = selectedItem.dataset.id; // Imposta l'ID dell'elemento selezionato nel campo nascosto
    flagRef.value = true; // Imposta il flag che indica che un elemento è stato selezionato
    showError(inputElement, errorElement, true); // Mostra eventuali errori
    document.querySelectorAll('.search-results').forEach(results => {
        results.style.display = 'none'; // Nasconde la lista dei risultati di ricerca
    });
}

// Gestisce la ricerca di giochi mentre l'utente digita
const gameSelected = { value: false };
gameSearch.addEventListener('input', function () {
    gameSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchGames?query=${encodeURIComponent(searchQuery)}`) // Chiede i giochi tramite una richiesta fetch
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

                        gameSearchResults.appendChild(div); // Aggiunge ogni gioco come un risultato nella lista
                    });
                    gameSearchResults.style.display = 'block'; // Mostra la lista dei risultati
                } else {
                    gameSearchResults.style.display = 'none'; // Nasconde la lista se non ci sono risultati
                }
            });
    } else {
        gameSearchResults.style.display = 'none'; // Nasconde la lista se non c'è alcun input
    }
});

// Funzione di ricerca per store
const storeSelected = { value: false };
storeSearch.addEventListener('input', function () {
    storeSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchStores?query=${encodeURIComponent(searchQuery)}`) // Chiede gli store tramite una richiesta fetch
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
                    storeSearchResults.style.display = 'block'; // Mostra i risultati
                } else {
                    storeSearchResults.style.display = 'none'; // Nasconde i risultati se non ce ne sono
                }
            });
    } else {
        storeSearchResults.style.display = 'none'; // Nasconde i risultati se non c'è alcun input
    }
});

// Funzione di ricerca per piattaforme
const platformSelected = { value: false };
platformSearch.addEventListener('input', function () {
    platformSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchPlatforms?query=${encodeURIComponent(searchQuery)}`) // Chiede le piattaforme tramite una richiesta fetch
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
                    platformSearchResults.style.display = 'block'; // Mostra i risultati
                } else {
                    platformSearchResults.style.display = 'none'; // Nasconde i risultati se non ce ne sono
                }
            });
    } else {
        platformSearchResults.style.display = 'none'; // Nasconde i risultati se non c'è alcun input
    }
});

// Funzione di ricerca per launcher
const launcherSelected = { value: false };
launcherSearch.addEventListener('input', function () {
    launcherSelected.value = false;
    const searchQuery = this.value.trim().toLowerCase();
    if (searchQuery.length > 0) {
        fetch(`/Games/SearchLaunchers?query=${encodeURIComponent(searchQuery)}`) // Chiede i launcher tramite una richiesta fetch
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
                    launcherSearchResults.style.display = 'block'; // Mostra i risultati 
                } else {
                    launcherSearchResults.style.display = 'none'; // Nasconde i risultati se non ce ne sono
                }
            });
    } else {
        launcherSearchResults.style.display = 'none'; // Nasconde i risultati se non c'è alcun input
    }
});

// Gestisce la chiusura dei risultati di ricerca quando si clicca fuori
document.addEventListener('click', function (e) {
    if (!e.target.closest('.search-container')) {
        document.querySelectorAll('.search-results').forEach(results => {
            results.style.display = 'none'; // Nasconde la lista dei risultati quando si clicca fuori
        });
    }
});

// Funzione per nascondere i messaggi di successo e errore dopo 3 secondi
document.addEventListener("DOMContentLoaded", function () {
    const successMessage = document.querySelector(".alert-success");
    if (successMessage) {
        setTimeout(() => {
            successMessage.style.display = "none";
        }, 3000); // Nasconde il messaggio dopo 3 secondi
    }

    const errorMessage = document.querySelector(".alert-danger");
    if (errorMessage) {
        setTimeout(() => {
            errorMessage.style.display = "none";
        }, 3000); // Nasconde il messaggio dopo 3 secondi
    }
});

// Gestisce la validazione del campo store
document.getElementById('storeSearch').addEventListener('input', function () {
    const inputValue = this.value;
    validateStoreInput(inputValue); // Chiama la funzione di validazione
});

// Funzione di validazione per il campo store
function validateStoreInput(inputValue) {
    const errorMessageElement = document.getElementById('storeSearchError');
    const storeInput = document.getElementById('storeSearch');

    if (existingStores.includes(inputValue)) {
        errorMessageElement.style.display = 'none'; // Se lo store esiste, nasconde l'errore
        storeInput.classList.remove('is-invalid'); // Rimuove la classe di errore
    } else {
        errorMessageElement.style.display = 'block'; // Mostra un errore se lo store non esiste
        errorMessageElement.innerHTML = 'Store not found. Please select from the dropdown or create a new store.';
        storeInput.classList.add('is-invalid'); // Aggiunge la classe di errore
    }
}

// Mostra i suggerimenti nel dropdown per store
function showDropdownSuggestions() {
    const storeSearchResults = document.getElementById('storeSearchResults');
    storeSearchResults.style.display = 'block';
}

// Mostra il dropdown quando l'utente scrive
document.getElementById('storeSearch').addEventListener('input', function () {
    showDropdownSuggestions();
});

// Imposta la data odierna nel campo "purchaseDate"
document.addEventListener("DOMContentLoaded", function () {
    const purchaseDateInput = document.getElementById("purchaseDate");
    const today = new Date().toISOString().split("T")[0]; // Ottiene la data odierna in formato yyyy-mm-dd
    purchaseDateInput.value = today; // Imposta la data odierna nel campo
});
