<template>
  <div class="knowledge-base">
    <v-tabs
      v-model="tabsModel"
      color="grey lighten-4"
      align-with-title
    >
      <v-tabs-slider color="primary" />

      <v-tab
        v-for="(tab, i) in tabs"
        :key="i"
      >
        {{tab.name}}
        <!-- // TODO: add popup for when there a too many tabs -->
      </v-tab>

      <NewTabDialog />

      <v-tab-item
        v-for="(tab, i) in tabs"
        :key="i"
      >
        <TabItems v-bind:items="tab.items" />
        <NewTabItemDialog />
      </v-tab-item>
    </v-tabs>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import TabItems from '@/components/TabItems.vue'
import NewTabDialog from '@/components/dialogs/NewTabDialog.vue'
import NewTabItemDialog from '@/components/dialogs/NewTabItemDialog.vue'
import axios from '@/tools/axios'
export default Vue.extend({
  components: { TabItems, NewTabDialog, NewTabItemDialog },
  data() {
    return { newTabDialog: null,
      newTabItemDialog: null,
      tabsModel: null,
      tabs: null
      };
  },
  mounted(){
    axios.get(`/wiki`)
      .then(response => this.tabs = response.data )
      .catch( err => this.tabs = null);
  }
})
</script>
