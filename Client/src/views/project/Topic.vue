<template>
  <v-container fluid>
    <v-layout wrap>

      <TopicArticles />

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import axios from 'axios'
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import TopicArticles from '../../components/TopicArticles.vue'

@Component({
  components: {
    TopicArticles,
  }
})
export default class Topic extends Vue {
}
</script>
