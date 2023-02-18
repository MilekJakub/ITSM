function generateBoard(model) {

    const changeBtn = document.getElementById('change');

    const selectEl = document.getElementById('projectId');
    selectEl.addEventListener('change', function () {
        changeBtn.click();
    });

    const generatedBoards = [];

    function getItemsForBoard(input) {

        const workItems = [];

        if (input == null) {
            for (const item of model.WorkItems) {
                if (item.StateId == null) {
                    const workItem =
                    {
                        id: `${item.Id}`,
                        title: `${item.Title}`,
                        click: function () {
                            location.href = window.location.origin + '/WorkItem/Details/' + item.Id;
                        }
                    }
                    workItems.push(workItem);
                }
            }
            return workItems;
        }
        else {
            for (const item of model.WorkItems) {
                if (item.StateId == input.Id) {
                    const workItem =
                    {
                        id: `${item.Id}`,
                        title: `${item.Title}`,
                        click: function (el) {
                            location.href = window.location.origin + '/WorkItem/Details/' + item.Id;
                        }
                    }

                    workItems.push(workItem);
                }
            }

            return workItems;
        }
    }

    const nullStateItems = getItemsForBoard(null);

    const nullsBoard = {
        id: `0`,
        title: "Not assigned",
        class: "text-dark",
        item: nullStateItems
    };

    generatedBoards.push(nullsBoard);

    for (const state of model.States) {

        const boardItems = getItemsForBoard(state);

        const board = {
            id: `${state.Id}`,
            title: state.Name,
            class: "text-dark",
            dragTo: [`${state.Id + 1}`, `${state.Id - 1}`],
            item: boardItems
        };

        generatedBoards.push(board);
    }

    const changes = [];
    let unsavedChanges = false;

    window.onload = function () {
        window.addEventListener("beforeunload", function (e) {
            if (unsavedChanges) {
                e.preventDefault();
                e.returnValue = '';
            }
        });
    };

    var KanbanTest = new jKanban({
        element: "#myKanban",
        gutter: "10px",
        widthBoard: "450px",
        dragBoards: false,
        itemHandleOptions: {
            enabled: true,
        },
        dropEl: function (el, target) {

            const stateId = target.closest('div.kanban-board').getAttribute('data-id');
            const workItemId = el.getAttribute('data-eid');

            const change = { workItem: workItemId, toState: stateId };

            changes.push(change);

            document.querySelector('#save').classList.remove('d-none');
            unsavedChanges = true;
        },
        boards: generatedBoards

    });

    document.querySelector('main').style.overflow = 'scroll';

    const slider = document.querySelector('main');
    let isDown = false;
    let startX;
    let scrollLeft;

    slider.addEventListener('mousedown', (e) => {
        isDown = true;
        slider.classList.add('active');
        startX = e.pageX - slider.offsetLeft;
        scrollLeft = slider.scrollLeft;
    });
    slider.addEventListener('mouseleave', () => {
        isDown = false;
        slider.classList.remove('active');
    });
    slider.addEventListener('mouseup', () => {
        isDown = false;
        slider.classList.remove('active');
    });
    slider.addEventListener('mousemove', (e) => {
        if (!isDown) return;
        e.preventDefault();
        const x = e.pageX - slider.offsetLeft;
        const walk = (x - startX) * 3; //scroll-fast
        slider.scrollLeft = scrollLeft - walk;
    });

    const saveBtn = document.getElementById('save');

    saveBtn.addEventListener('click', async function () {
        await saveChangesRequest(changes)
            .then(function () {
                unsavedChanges = false;
                window.location.reload()
            });
    });

    async function saveChangesRequest(changesToRequest) {
        const url = window.location.origin + '/Home/ApproveChanges';

        const requestOptions = {
            method: "POST",
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ Data: changesToRequest }),
            redirect: "manual",
            referrerPolicy: "no-referrer",
        };

        return await fetch(url, requestOptions);
    }

}
