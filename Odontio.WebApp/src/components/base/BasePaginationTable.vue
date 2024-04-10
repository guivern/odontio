<template>
  <div>
    <base-card>
      <v-container>
        <v-row
          class="d-flex align-center"
          v-if="!hideSearchInput || !hideCreateButton"
        >
          <v-col cols="12" lg="6" md="6">
            <base-input
              v-if="!hideSearchInput"
              label="Buscar"
              v-model="searchCopy"
              prepend-inner-icon="mdi-magnify"
              type="search"
              @update:model-value="onSearchInput($event)"
            />
          </v-col>

          <slot name="prepend-actions"></slot>
          <slot name="actions">
            <!-- <v-col cols="4" lg="3" md="3"> -->
              <!-- <slot name="prepend-create-button"></slot> -->
              <!-- <v-btn
                v-if="!hideCreateButton"
                color="primary"
                @click="onCreateClick"
                :disabled="disabledCreateButton"
                prepend-icon="mdi-plus"
                flat
              >
              Nuevo
              </v-btn> -->
              <!-- <slot name="append-create-button"></slot> -->
            <!-- </v-col> -->
          </slot>
          <slot name="append-actions"></slot>
        </v-row>
        <v-row>
          <v-col cols="12">
            <v-data-table-server v-bind="$attrs" @click:row="onRowClick">
              <template
                v-for="(_, slotName) in $slots"
                v-slot:[slotName]="slotProps"
              >
                <slot :name="slotName" v-bind="slotProps" />
              </template>
            </v-data-table-server>
          </v-col>
        </v-row>
      </v-container>
    </base-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { computed } from "vue";
import { useDisplay } from "vuetify";

const { mobile } = useDisplay();

const props = defineProps({
  search: {
    type: String,
    required: false,
  },
  disabledCreateButton: {
    type: Boolean,
    required: false,
  },
  hideCreateButton: {
    type: Boolean,
    required: false,
  },
  hideSearchInput: {
    type: Boolean,
    required: false,
  },
});

const emits = defineEmits([
  "click:row",
  "search",
  "click:create",
]);

const onRowClick = (event: any, { item }: { item: any }) => {
  emits("click:row", item);
};

const onCreateClick = () => {
  emits("click:create");
};

const searchCopy = ref(props.search);

let timer: any;
const onSearchInput = (input: any) => {
  clearTimeout(timer);
  timer = setTimeout(async () => {
    emits("search", input);
  }, 800);
};
</script>
