/**
 * Custom component created to avoid code repetition (DRY: Don't Repeat Yourself)
 * This component is used to render the nav element in the top part of the body element
 */
class Nav extends HTMLElement {
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
            <nav class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0 shadow">
                <a class="navbar-brand col-md-3 col-lg-2 mr-0 px-3" href="#">Log Manager v1.0</a>
                <button class="navbar-toggler position-absolute d-md-none collapsed" type="button" data-toggle="collapse"
                    data-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
            </nav>
        `
    }
  }

customElements.define('header-nav', Nav);