import vuetify from "./vuetify";
import type { App } from "vue";

// Components
import BaseInput from "@/components/base/BaseInput.vue";
import BaseSelect from "@/components/base/BaseSelect.vue";
import BaseTextarea from "@/components/base/BaseTextarea.vue";
import BaseCheckbox from "@/components/base/BaseCheckbox.vue";
import BaseCard from "@/components/base/BaseCard.vue";
import BaseAutocomplete from "@/components/base/BaseAutocomplete.vue";
import BasePaginationTable from "@/components/base/BasePaginationTable.vue";

export function registerComponents(app: App) {
  app.component("BaseInput", BaseInput);
  app.component("BaseSelect", BaseSelect);
  app.component("BaseTextarea", BaseTextarea);
  app.component("BaseCheckbox", BaseCheckbox);
  app.component("BaseCard", BaseCard);
  app.component("BaseAutocomplete", BaseAutocomplete);
  app.component("BasePaginationTable", BasePaginationTable);
}