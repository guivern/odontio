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
import ToggleReadMode from '@/components/shared/ToggleReadMode.vue';
import ErrorAlert from '@/components/shared/ErrorAlert.vue';
import FormActions from "@/components/shared/FormActions.vue";
import UiParentCard from '@/components/shared/UiParentCard.vue';
import FormActionsToolbar from "@/components/shared/FormActionsToolbar.vue";
import BaseAlert from "@/components/base/BaseAlert.vue";
import DialogAlert from "@/components/shared/DialogAlert.vue";
import BaseBreadcrumb from "@/components/shared/BaseBreadcrumb.vue";
import BaseDateInput from "@/components/base/BaseDateInput.vue";
import BaseTabs from "@/components/base/BaseTabs.vue";

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
  app.component("ToggleReadMode", ToggleReadMode);
  app.component("ErrorAlert", ErrorAlert);
  app.component("FormActions", FormActions);
  app.component("UiParentCard", UiParentCard);
  app.component("FormActionsToolbar", FormActionsToolbar);
  app.component("BaseAlert", BaseAlert);
  app.component("DialogAlert", DialogAlert);
  app.component("BaseBreadcrumb", BaseBreadcrumb);
  app.component("BaseDateInput", BaseDateInput);
  app.component("BaseTabs", BaseTabs);
}