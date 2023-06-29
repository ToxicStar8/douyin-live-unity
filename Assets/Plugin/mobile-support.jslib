mergeInto(LibraryManager.library, {
  //弹出js原生输入框
  Prompt: function (name, message, defaultValue) {
    //if(UnityLoader.SystemInfo.mobile){
    const name_str = UTF8ToString(name);
    const message_str = UTF8ToString(message);
    const defaultValue_str = UTF8ToString(defaultValue);
    const result = window.prompt(message_str, defaultValue_str);
    if (result === null) {
      SendMessage(name_str, 'OnPromptCancel');
    } else {
      SendMessage(name_str, 'OnPromptOk', result);
    }
    //}
  },

  //复制文本
  CopyText: function (text) {
    navigator.clipboard.writeText(UTF8ToString(text));
  }
});