document.addEventListener("DOMContentLoaded", function () {

   // Navbar brand href updating

   var navbarBrand = document.querySelector('.navbar-brand');
    
   if (navbarBrand) {
      navbarBrand.setAttribute('href', 'https://opensettings.net');   
   }

    // Version dropdown 

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

        versions.forEach(function (version) {
            var menuItem = document.createElement('li');
            var menuLink = document.createElement('a');
            menuLink.className = 'dropdown-item';
            menuLink.href = '#';
            menuLink.textContent = 'v' + version;

            menuLink.addEventListener('click', function (event) {
                event.preventDefault();
                var currentPath = window.location.pathname;
                var versionPattern = /\/v\d+\//;
                var newPath = versionPattern.test(currentPath) 
                    ? currentPath.replace(versionPattern, `/v${version}/`) 
                    : `/v${version}${currentPath}`;

                window.location.href = newPath;
            });

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
            addVersionDropdown();
        }
    }, 500);

    var observer = new MutationObserver(function () {
        if (document.querySelector('.icons')) {
            observer.disconnect();
            addVersionDropdown();
        }
    });

    observer.observe(document.body, { childList: true, subtree: true });
});
