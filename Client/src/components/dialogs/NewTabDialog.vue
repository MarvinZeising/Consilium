<template>
  <v-layout>
    <v-dialog v-model="newTabDialog" persistent max-width="600px">
      <template v-slot:activator="{ on }">
        <v-tab v-on="on">
          <v-icon color="secondary">add</v-icon>
        </v-tab>
      </template>
      <v-card>
        <v-card-title>
          <span class="headline">Add new Tab</span>
        </v-card-title>
        <v-card-text>
          <v-text-field
            v-model="tabName"
            label="Tab name"
            box
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            flat
            color="black"
            @click="newTabDialog = false"
            v-t="'core.close'"
          />
          <v-btn
            :disabled="tabName == ''"
            flat
            color="primary"
            @click="newTabDialog = false"
            v-t="'core.save'"
          />
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      newTabDialog: null,
      tabName: '',
    }
  }
})
</script>
