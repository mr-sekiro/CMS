window.addEventListener('DOMContentLoaded', event => {
    // --- Theme Switcher Functionality ---
    const themeSwitch = document.body.querySelector('#themeSwitch');
    const htmlEl = document.documentElement;

    const setTheme = (theme) => {
        htmlEl.setAttribute('data-bs-theme', theme);
        localStorage.setItem('theme', theme);
        if (themeSwitch) {
            themeSwitch.checked = theme === 'dark';
        }
    };

    const savedTheme = localStorage.getItem('theme') || 'light';
    setTheme(savedTheme);

    if (themeSwitch) {
        themeSwitch.addEventListener('change', () => {
            const newTheme = themeSwitch.checked ? 'dark' : 'light';
            setTheme(newTheme);
        });
    }

    // --- Sidebar Toggle Functionality (REVISED & FIXED) ---
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    const wrapper = document.body.querySelector('#wrapper');

    const applySidebarState = () => {
        const isToggled = localStorage.getItem('sidebar-toggled') === 'true';


        if (window.innerWidth < 768) {
            // On mobile, we always start with the sidebar hidden.
            wrapper.classList.add('toggled');
        } else {
            // On desktop, respect the user's last choice.
            if (isToggled) {
                wrapper.classList.add('toggled');
            } else {
                wrapper.classList.remove('toggled');
            }
        }
    };

    const applySidebarStateRevised = () => {
        const isSidebarVisible = localStorage.getItem('sidebar-visible') === 'true';

        if (window.innerWidth >= 768) { // Desktop view
            if (isSidebarVisible) {
                wrapper.classList.remove('toggled');
            } else {
                // Default to visible on desktop
                if (localStorage.getItem('sidebar-visible') === null) {
                    wrapper.classList.remove('toggled');
                } else {
                    wrapper.classList.add('toggled');
                }
            }
        }
        // Mobile view is handled by CSS media queries primarily.
    };

    // Let's use the first simpler logic, but correct it.
    const finalSidebarLogic = () => {
        const isToggled = localStorage.getItem('sidebar-toggled') === 'true'; // Toggled means hidden on desktop

        // Set initial state based on screen size and saved preference
        if (window.innerWidth < 768) {
            wrapper.classList.add('toggled'); // Always start hidden on mobile
        } else {
            if (isToggled) {
                wrapper.classList.add('toggled');
            } else {
                wrapper.classList.remove('toggled');
            }
        }
    };

    const applySidebarStateFinal = () => {
        const isToggled = localStorage.getItem('sidebar-toggled') === 'true';
        const isMobile = window.innerWidth < 768;

        if (isMobile) {
            // On mobile, the sidebar should always start off-screen.
            wrapper.classList.add('toggled');
        } else {
            // On desktop, respect the user's choice.
            if (isToggled) {
                wrapper.classList.add('toggled');
            } else {
                wrapper.classList.remove('toggled');
            }
        }
    };

    applySidebarStateFinal();


    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            wrapper.classList.toggle('toggled');
            localStorage.setItem('sidebar-toggled', wrapper.classList.contains('toggled'));
        });
    }


    // --- Active Sidebar Link Functionality ---
    const sidebarLinks = document.querySelectorAll('#sidebar-wrapper .list-group-item, #sidebar-wrapper .collapse-item');
    const currentPath = window.location.pathname;
    let linkFound = false;

    sidebarLinks.forEach(link => {
        link.classList.remove('active');
        const linkPath = link.getAttribute('href');

        if (linkPath && linkPath !== "/" && currentPath.toLowerCase().startsWith(linkPath.toLowerCase())) {
            link.classList.add('active');
            linkFound = true;

            const parentCollapse = link.closest('.collapse');
            if (parentCollapse) {
                parentCollapse.classList.add('show');
                const parentToggler = document.querySelector(`a[href="#${parentCollapse.id}"]`);
                if (parentToggler) {
                    parentToggler.setAttribute('aria-expanded', 'true');
                }
            }
        }
    });

    if (!linkFound && currentPath === '/') {
        const dashboardLink = document.querySelector('a[asp-controller="Home"][asp-action="Index"]');
        if (dashboardLink) {
            dashboardLink.classList.add('active');
        }
    }
});