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
     * Create masks for log date input
     */
  initializeDatePicker: (id) => {
    $(id).datetimepicker({
      mask: '9999/12/31 23:59',
    });
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

    onSearchSubmit: () => {
      $("#form-search").submit( e => {
        e.preventDefault();

        const logViewModel = {
          IPAddress: $("#search-ip").val(),
          initialDate: $("#search-initial-date").val(),
          endDate: $("#search-end-date").val()
        }        

        $.ajax({
          url: "https://localhost:5001/api/logs/search",
          method: "POST",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          data: JSON.stringify(logViewModel)
        })
        .done(d => {
          $('#table-logs').DataTable().clear();
          $('#table-logs').DataTable().rows.add(d);
          $('#table-logs').DataTable().draw();
        });
      });
    },

    /**
     * Initialize the DataTables.net component with all logs
     */
    loadTable: () => {
      $.ajax({
        url: "https://localhost:5001/api/logs/",
      })
      .done( d => {
        $('#table-logs').DataTable({
          dom: "lrtip",
          data: d,
          columns: [
            { data: "id" },
            { data: "ipAddress" },
            { data: "logDate" },
            { data: "logMessage" },
            {
              data: null,
              orderable: false,
              render: function (data, type, row, meta) {
                return '<a href="/edit.html?id=' + row.id + '">View Details</a>'
              }
            }
          ]
        })
      });
    }
  },

  /**
   * Functions related to the add log page
   */
  AddLog: {
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
      $("#form-add-log").submit(e => {
          e.preventDefault();

          $("#submit-log").prop("disabled", true);

          const logViewModel = {
            IPAddress: $("#log-ip").val(),
            LogDate: $("#log-date").val(),
            LogMessage: $("#log-message").val()
          }

          $.ajax({
            url: "https://localhost:5001/api/logs/",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(logViewModel)
          })
            .done(() => {
              MainScript.redirect("#bt-log-list");
            })
            .fail((i, e) => {
              $("#submit-feedback").html("Error processing request: " + i.responseText);
              $("#submit-log").prop("disabled", false);
            })
        }
      );
    }
  },

  /**
   * Functions related to the edit log page
   */
  EditLog: {
    /**
     * Set the click event action for the navigation buttons
     */
    onButtonClick: () => {
      $(document).on("click", "#bt-upload-files, #bt-log-list", (e) => {
        MainScript.redirect(e.target.id);
      });
    },

    /**
     * get the log data based on it's id
     */
    loadData: () => {
      const searchParams = new URLSearchParams(window.location.search);

      $.ajax({
        url: "https://localhost:5001/api/logs/" + searchParams.get("id"),
        method: "GET"
      })
        .done((data) => {
          $("#log-id").val(data.id);
          $("#log-ip").val(data.ipAddress);
          $("#log-date").val(data.logDate);
          $("#log-message").val(data.logMessage);
        });
    },


    /**
     * Set the click event action for the update log button
     */
    onUpdateLogClicked: () => {
      $("#update-log").on('click', e => {
        $("#update-log, #delete-log").prop("disabled", true);

        const id = $("#log-id").val();

        const logViewModel = {
          Id: id,
          IPAddress: $("#log-ip").val(),
          LogDate: $("#log-date").val(),
          LogMessage: $("#log-message").val()
        }

        $.ajax({
          url: "https://localhost:5001/api/logs/" + id,
          method: "PUT",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          data: JSON.stringify(logViewModel)
        })
          .done(() => {
            MainScript.redirect("#bt-log-list");
          })
          .fail((i, e) => {
            $("#submit-feedback").html("Error processing request: " + i.responseText);
            $("#update-log, #delete-log").prop("disabled", false);
          })
      });
    },

    /**
       * Set the click event action for the delete log button
       */
    onDeleteLogClicked: () => {
      $("#delete-log").on('click', e => {
        $("#update-log, #delete-log").prop("disabled", true);

        const id = $("#log-id").val();

        $.ajax({
          url: "https://localhost:5001/api/logs/" + id,
          method: "DELETE"
        })
          .done(() => {
            MainScript.redirect("#bt-log-list");
          })
          .fail((i, e) => {
            $("#submit-feedback").html("Error processing request: " + i.responseText);
            $("#update-log, #delete-log").prop("disabled", false);
          })
      });
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

        for (let i = 0, f; f = e.target.files[i]; i++) {
          fileName += f.name + ",";
        }

        $('.custom-file-label').html(fileName);
      })
    },

    /**
     * Set the click event action for the submit button
     */
    onFormSubmit: () => {
      $("#form-upload-files").submit(e => {
          e.preventDefault();

          $("#send-log-files").prop("disabled", true);

          const fileupload = $("#customFile").get(0);
          const files = fileupload.files;
          let data = new FormData();
          for (let i = 0, f; f = files[i]; i++) {
            data.append("files", f);
          }

          // const logViewModel = {
          //   IPAddress: $("#log-ip").val(),
          //   LogDate: $("#log-date").val(),
          //   LogMessage: $("#log-message").val()
          // }

          $.ajax({
            url: "https://localhost:5001/api/logs/PostFile",
            method: "POST",
            // contentType: "application/json; charset=utf-8",
            // dataType: "json",
            // data: JSON.stringify(logViewModel)
            contentType: false,
            processData: false,
            data: data
          })
            .done(() => {
              MainScript.redirect("#bt-log-list");
            })
            .fail((i, e) => {
              $("#submit-feedback").html("Error processing request: " + i.responseText);
              $("#send-log-files").prop("disabled", false);
            })
        }
      );
    }
  }
}