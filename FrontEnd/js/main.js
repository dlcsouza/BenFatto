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
      case "nav-add-log":
        location.href = "./add.html";
        break;

      case "bt-upload-files":
      case "nav-upload-files":
        location.href = "./upload.html";
        break;

      default:
        // bt-log-list, nav-log-list
        location.href = "./index.html";
    }
  },

  /**
   * Add the "active" class to the menu item that represents the current page being viewed
   */
  setActiveMenu: (id) => {
    $(id).addClass("active");
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
     * Create masks for the IP and log date inputs
     */
    initialize: () => {
      $('#log-date').datetimepicker({
        mask:'9999/12/31 23:59',
      });
    },
    
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
          e.preventDefault();
          
          $("#submit-log-message").prop("disabled", true);

          const logViewModel = {
              IPAddress: $("#log-ip").val(),
              LogDate: $("#log-date").val(),
              LogMessage: $("#log-message").val()
          }

          $.ajax({
            url: "https://localhost:5001/api/logs/",
            method: "POST",
            contentType:"application/json; charset=utf-8",
            dataType:"json",
            data: JSON.stringify(logViewModel)
          })
          .done(() => {
            MainScript.redirect("#bt-log-list");
          })
          .fail((i, e) => {
            $("#submit-feedback").html("Error processing request: " + e);
            $("#submit-log-message").prop("disabled", false);
          })
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
     * Sets the selected files' names into the input
     */
    onInputChange: () => {
      $('#customFile').on('change', (e) => {
        let fileName = "";
        
        e.target.forEach( (e, i, a) => {
            fileName += e.name + ",";
          }
        )

        $(this).next('.custom-file-label').html(fileName);
    })
    },

    /**
     * Set the click event action for the submit button
     */
    onFormSubmit: () => {
      $("#form-add-log").submit(
        e => {
          e.preventDefault();
          
          $("#submit-log-message").prop("disabled", true);

          const logViewModel = {
              IPAddress: $("#log-ip").val(),
              LogDate: $("#log-date").val(),
              LogMessage: $("#log-message").val()
          }

          $.ajax({
            url: "https://localhost:5001/api/logs/",
            method: "POST",
            contentType:"application/json; charset=utf-8",
            dataType:"json",
            data: JSON.stringify(logViewModel)
          })
          .done(() => {
            MainScript.redirect("#bt-log-list");
          })
          .fail((i, e) => {
            $("#submit-feedback").html("Error processing request: " + e);
            $("#submit-log-message").prop("disabled", false);
          })
        }
      );
    }
  }
}