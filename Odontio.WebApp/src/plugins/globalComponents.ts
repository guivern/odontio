import vuetify from "./vuetify";
import type { App } from "vue";

// Components
import BaseTextInput from "@/components/base/BaseTextInput.vue";
import BaseSelect from "@/components/base/BaseSelect.vue";
import BaseTextarea from "@/components/base/BaseTextarea.vue";
import BaseCheckbox from "@/components/base/BaseCheckbox.vue";
import BaseCard from "@/components/base/BaseCard.vue";
import BaseAutocomplete from "@/components/base/BaseAutocomplete.vue";
import BasePaginationTable from "@/components/base/BasePaginationTable.vue";
import ErrorCard from "@/components/shared/ErrorCard.vue";
import DeleteDialog from "@/components/shared/DeleteDialog.vue";

export function registerComponents(app: App) {
  app.component("BaseTextInput", BaseTextInput);
  app.component("BaseSelect", BaseSelect);
  app.component("BaseTextarea", BaseTextarea);
  app.component("BaseCheckbox", BaseCheckbox);
  app.component("BaseCard", BaseCard);
  app.component("BaseAutocomplete", BaseAutocomplete);
  app.component("BasePaginationTable", BasePaginationTable);
  app.component("ErrorCard", ErrorCard);
  app.component("DeleteDialog", DeleteDialog);
}