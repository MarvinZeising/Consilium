<template>
  <div class="ma-3">
    <v-expansion-panel :expand=true>
      <v-expansion-panel-content
        v-for="(item, itemPosition) in items"
        :key="item.title"
      >
        <template v-slot:header>
          <div v-text="item.title" />
        </template>
        <v-card>
          <v-card-text
            v-text="item.text"
            class="grey lighten-4"
          />
          <v-card-actions>
            <v-spacer />
            <v-btn
              v-if="itemPosition != 0"
              flat
              color="secondary"
            >
              <v-icon class="mr-2">arrow_upward</v-icon>
              Move up
            </v-btn>
            <v-btn
              v-if="itemPosition != items.length - 1"
              flat
              color="secondary"
            >
              <v-icon class="mr-2">arrow_downward</v-icon>
              Move down
            </v-btn>
            <v-btn flat color="primary">
              <v-icon class="mr-2">edit</v-icon>
              Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-expansion-panel-content>
    </v-expansion-panel>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  props: ['items']
})
</script>
