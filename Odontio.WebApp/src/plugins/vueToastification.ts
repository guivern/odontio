import type { App } from 'vue';
import Toast from 'vue-toastification';
import { POSITION } from 'vue-toastification';
import type { PluginOptions } from 'vue-toastification';
import 'vue-toastification/dist/index.css';

const filterBeforeCreate = (toast: any, toasts: any) => {
  if (toasts.filter((t: any) => t.type === toast.type).length !== 0) {
    // Returning false discards the toast
    return false;
  }
  // You can modify the toast if you want
  return toast;
};

const toastOptions: PluginOptions = {
  position: POSITION.BOTTOM_CENTER,
  filterBeforeCreate: filterBeforeCreate,
  hideProgressBar: true
};

export function registerToast(app: App) {
  app.use(Toast, toastOptions);
}
