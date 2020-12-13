/**
 * Functionality to manage the behavior of the website elements, such as buttons and tables
 */
MainScript = {
  /**
   * Redirect to a specific URL according to the id passed
   */
  redirect: (id) => {
    switch (id) {
      case "bt-add-log":
        location.href = "./add.html";
        break;

      case "bt-upload-files":
        location.href = "./upload.html";
        break;

      default:
        // bt-log-list
        location.href = "./index.html";
    }
  },

  /**
   * Functions related to the log list page
   */
  LogList: {
    /**
     * Set the click event action for the navigation buttons
     */
    onButtonClick: () => {
      $(document).on("click", "#bt-add-log, #bt-upload-files", (e) => {
        MainScript.redirect(e.target.id);
      });
    },

    /**
     * Initialize the DataTables.net component
     */
    loadTable: () => {
      $.ajax({
        url: "https://localhost:5001/api/logs/",
      })
      .done((d) => {
          console.log(d);

          $('#table-logs').DataTable({
            dom: "lrtip",
            data: d,
            columns: [
              {data: "id"},
              {data: "ipAddress"},
              {data: "logDate"},
              {data: "logMessage"},
            ]
          })
        }
      );
    }
  },

  /**
   * Functions related to the add / edit log page
   */
  AddEditLog: {
    /**
     * Set the click event action for the navigation buttons
     */
    onButtonClick: () => {
      $(document).on("click", "#bt-upload-files, #bt-log-list", (e) => {
        MainScript.redirect(e.target.id);
      });
    },

    /**
     * Set the click event action for the submit button
     */
    onFormSubmit: () => {
      $("#form-add-log").submit(
        e => {
          const logMessage = $('#log-message').val().trim();
          if (!logMessage) {
            $("#log-message-invalid-feedback").html("Valid log message is required");
            e.preventDefault();
          }
        }
      );
    }
  },

  /**
   * Functions related to the upload files page
   */
  UploadFiles: {
    /**
     * Set the click event action for the navigation buttons
     */
    onButtonClick: () => {
      $(document).on("click", "#bt-log-list, #bt-add-log", (e) => {
        MainScript.redirect(e.target.id);
      });
    },

    /**
     * Set the click event action for the submit button
     */
    onFormSubmit: () => {
      $("#form-add-log").submit(
        e => {
          alert("ToDo");
        }
      );
    }
  }
}