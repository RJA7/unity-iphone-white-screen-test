mergeInto(LibraryManager.library, {
  ClearCacheAndReload: function() {
    // Clear all caches
    caches.keys().then(function(names) {
      return Promise.all(names.map(function(name) {
        return caches.delete(name);
      }));
    }).then(function() {
      // Clear IndexedDB UnityCache
      var request = indexedDB.open('UnityCache');
      request.onsuccess = function(event) {
        var db = event.target.result;
        var storeNames = Array.from(db.objectStoreNames);
        if (storeNames.length === 0) {
          db.close();
          window.location.reload();
          return;
        }
        var transaction = db.transaction(storeNames, "readwrite");
        storeNames.forEach(function(storeName) {
          transaction.objectStore(storeName).clear();
        });
        transaction.oncomplete = function() {
          db.close();
          window.location.reload();
        };
        transaction.onerror = function() {
          console.error('Failed to clear IndexedDB');
          db.close();
          window.location.reload();
        };
      };
      request.onerror = function() {
        console.error('Failed to open IndexedDB');
        window.location.reload();
      };
    });
  }
});
