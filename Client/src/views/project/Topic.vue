<template>
  <div class="knowledge-base">
    <v-list two-line>
      <template v-for="(article, index) in articles">
        <v-list-tile
          :key="article.title"
          @click=""
        >
          <v-list-tile-content>
            <v-list-tile-title v-html="article.title"></v-list-tile-title>
            <v-list-tile-sub-title v-html="article.content"></v-list-tile-sub-title>
          </v-list-tile-content>
        </v-list-tile>

        <v-divider
          v-if="index > articles.length"
          :key="index"
          :inset="item.inset"
        ></v-divider>
      </template>
    </v-list>
  </div>
</template>

<script lang="ts">
import axios from 'axios'
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import KnowledgeBaseModule from '../../store/modules/knowledgeBase'
import { Topic, Article } from '../../models/definitions'

@Component
export default class Articles extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private topic: Topic | null = null
  private articles: Article[] = [{
    id: 'asdf',
    topicId: '1234',
    title: 'My first title',
    content: 'My awesome content'
  }]

  private async load() {
  }
}
</script>
