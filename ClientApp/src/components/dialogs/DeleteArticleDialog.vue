<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        color="error"
        v-t="'project.knowledgeBase.deleteArticle'"
      />
    </template>
    <v-card>
      <v-card-title>
        <span
          class="headline"
          v-t="'project.knowledgeBase.deleteArticle'"
        />
      </v-card-title>
      <v-card-text>
        <p
          class="subtitle-1"
          v-t="'project.knowledgeBase.deleteArticleDescription'"
        />
      </v-card-text>
      <v-card-actions>
        <v-btn
          text
          v-t="'core.cancel'"
          @click.stop="dialog = false"
        />
        <v-spacer />
        <v-btn
          type="submit"
          text
          color="error"
          v-t="'core.delete'"
          @click.stop="deleteArticle"
          :loading="loading"
        />
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Component, Watch, Prop, Vue } from 'vue-property-decorator'
import i18n from '../../i18n'
import KnowledgeBaseModule from '../../store/knowledgeBase'
import { Article } from '../../models'

@Component
export default class DeleteArticleDialog extends Vue {
  private knowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(Article)
  private readonly article?: Article

  private dialog = false
  private loading = false

  private async deleteArticle() {
    this.loading = true

    if (this.article) {
      await this.knowledgeBaseModule.deleteArticle(this.article.id)

      this.$router.push({ name: 'topic', params: this.$route.params })
    }

    this.loading = false
    this.dialog = false
  }

}
</script>
