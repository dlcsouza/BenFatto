/**
 * Custom component created to avoid code repetition (DRY: Don't Repeat Yourself)
 * This component is used to render the sidebar menu element in the top part of the body element
 */
class SidebarMenu extends HTMLElement {
    constructor() {
      super();
    }

    /**
     * From https://developer.mozilla.org/en-US/docs/Web/Web_Components/Using_custom_elements#Using_the_lifecycle_callbacks :
     * "Invoked each time the custom element is appended into a document-connected element. 
     * This will happen each time the node is moved, and may happen before the element's contents
     * have been fully parsed.""
     */
    connectedCallback() {
        this.innerHTML = `
            <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
            <div class="sidebar-sticky pt-3">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link active" href="#">
                            <span data-feather="home"></span>
                            Log list <span class="sr-only">(current)</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">
                            <span data-feather="file"></span>
                            Upload log files
                        </a>
                    </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#">
                                <span data-feather="shopping-cart"></span>
                                Add new log
                            </a>
                        </li>
                    </ul>
                </div>
            </nav>
        `
    }
  }

customElements.define('header-sidebar-menu', SidebarMenu);