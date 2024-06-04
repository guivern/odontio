import { createVuetify } from 'vuetify';
import '@mdi/font/css/materialdesignicons.css';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import { PurpleTheme, BlueTheme } from '@/theme/LightTheme';
import { VDateInput } from 'vuetify/labs/VDateInput'
// import { VFab } from 'vuetify/labs/VFab'

export default createVuetify({
  directives,
  components: {
    ...components,
    VDateInput,
    // VFab
  },

  theme: {
    defaultTheme: 'BlueTheme',
    themes: {
      BlueTheme,
      PurpleTheme
    }
  },
  defaults: {
    VBtn: {},
    VCard: {
      rounded: 'md'
    },
    VTextField: {
      rounded: 'lg'
    },
    VTooltip: {
      // set v-tooltip default location to top
      location: 'top'
    }
  }
});
