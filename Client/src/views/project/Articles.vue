<template>
  <div class="knowledge-base">
    <v-list two-line>
      <template v-for="(article, index) in articles">
        <v-list-item
          :key="article.title"
          @click=""
        >
          <v-list-item-content>
            <v-list-item-title v-html="article.title"></v-list-item-title>
            <v-list-item-subtitle v-html="article.content"></v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>

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

}
</script>
