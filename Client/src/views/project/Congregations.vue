<template>
  <v-container fluid>
    <v-layout wrap>

      <CongregationsList />

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'
import CongregationsList from '../../components/CongregationsList.vue'

@Component({
  components: {
    CongregationsList,
  }
})
export default class Congregations extends Vue {
}
</script>
