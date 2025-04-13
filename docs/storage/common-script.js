document.addEventListener("DOMContentLoaded", function () {

    // Navbar brand href updating

    var navbarBrand = document.querySelector('.navbar-brand');

    if (navbarBrand) {
        navbarBrand.setAttribute('href', 'https://opensettings.net');
    }

    // Github Repos dropdown

    var githubRepos = [
        {
            textContent: 'open-settings',
            name: 'open-settings',
            openInNewTab: true
        },
        {
            textContent: 'open-settings-spa',
            name: 'open-settings-spa',
            openInNewTab: true
        },
        {
            textContent: 'open-settings-docs',
            name: 'open-settings-docs',
            openInNewTab: true
        },
        {
            textContent: 'open-settings-docker',
            name: 'open-settings-docker',
            openInNewTab: true
        },
        {
            textContent: 'open-settings-samples',
            name: 'open-settings-samples',
            openInNewTab: true
        }
    ]

    function addGithubDropdown() {
        var navbar = document.querySelector('.icons');
        if (!navbar) return;

        if (document.querySelector('.github-dropdown')) return;

        var githubDropdown = document.createElement('div');
        githubDropdown.className = 'dropdown github-dropdown';

        var dropdownToggle = document.createElement('a');
        dropdownToggle.className = 'btn border-0 dropdown-toggle';
        dropdownToggle.setAttribute('data-bs-toggle', 'dropdown');
        dropdownToggle.setAttribute('aria-expanded', 'false');
        dropdownToggle.setAttribute('title', 'Select GitHub Repo');

        var dropdownIcon = document.createElement('i');
        dropdownIcon.className = 'bi bi-github';

        dropdownToggle.appendChild(dropdownIcon);

        var dropdownMenu = document.createElement('ul');
        dropdownMenu.className = 'dropdown-menu dropdown-menu-end';

        githubRepos.forEach(function (repo){
            var menuItem = document.createElement('li');
            var menuLink = document.createElement('a');
            
            menuLink.className = 'dropdown-item';
            menuLink.href = `https://github.com/OpenSettings/${repo.name}`;
            menuLink.textContent = repo.textContent;
            menuLink.target = repo.openInNewTab ? '_blank' : '_self';

            menuItem.appendChild(menuLink);
            dropdownMenu.appendChild(menuItem);
        });

        githubDropdown.appendChild(dropdownToggle);
        githubDropdown.appendChild(dropdownMenu);
        navbar.appendChild(githubDropdown);
    }

    // Versions dropdown 

    var versions = ["1"];

    function addVersionDropdown() {
        var navbar = document.querySelector('.icons');
        if (!navbar) return;

        if (document.querySelector('.version-dropdown')) return;

        var versionDropdown = document.createElement('div');
        versionDropdown.className = 'dropdown version-dropdown';

        var dropdownToggle = document.createElement('a');
        dropdownToggle.className = 'btn border-0 dropdown-toggle';
        dropdownToggle.setAttribute('data-bs-toggle', 'dropdown');
        dropdownToggle.setAttribute('aria-expanded', 'false');
        dropdownToggle.setAttribute('title', 'Select Version');

        var currentPath = window.location.pathname;
        var versionPattern = /\/v(\d+)\//;
        var matchedVersion = currentPath.match(versionPattern);
        var currentVersion = matchedVersion ? matchedVersion[1] : versions[0];
        dropdownToggle.textContent = 'v' + currentVersion;

        var dropdownMenu = document.createElement('ul');
        dropdownMenu.className = 'dropdown-menu dropdown-menu-end';

        var currentPath = window.location.pathname;
        var versionPattern = /\/v\d+\//;

        versions.forEach(function (version) {

            var newPath = versionPattern.test(currentPath)
                ? currentPath.replace(versionPattern, `/v${version}/`)
                : `/v${version}${currentPath}`;

            var menuItem = document.createElement('li');
            var menuLink = document.createElement('a');

            menuLink.className = 'dropdown-item';
            menuLink.href = newPath;
            menuLink.textContent = 'v' + version;

            menuItem.appendChild(menuLink);
            dropdownMenu.appendChild(menuItem);
        });

        versionDropdown.appendChild(dropdownToggle);
        versionDropdown.appendChild(dropdownMenu);
        navbar.appendChild(versionDropdown);
    }

    var interval = setInterval(function () {
        if (document.querySelector('.icons')) {
            clearInterval(interval);
            addGithubDropdown();
            addVersionDropdown();
        }
    }, 500);

    var observer = new MutationObserver(function () {
        if (document.querySelector('.icons')) {
            observer.disconnect();
            addGithubDropdown();
            addVersionDropdown();
        }
    });

    observer.observe(document.body, { childList: true, subtree: true });
});