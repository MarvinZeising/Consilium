<template>
  <v-dialog
    v-model="dialog"
    max-width="1000px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
      >
        <v-icon>edit</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.knowledgeBase.updateArticle'"
          />
        </v-card-title>
        <v-card-text>
          <span class="subtitle-1">
            {{ $t('project.knowledgeBase.updateArticleDescription') }}
            {{ $t('project.knowledgeBase.markdownHint') }}
            <a
              href="https://guides.github.com/features/mastering-markdown"
              target="blank"
            >
              guides.github.com/features/mastering-markdown
            </a>
          </span>
        </v-card-text>
        <v-card-text>
          <p v-t="'project.knowledgeBase.titleDescription'" />
          <v-text-field
            v-model="title"
            :rules="titleRules"
            :label="$t('project.knowledgeBase.title')"
            :counter="title.length >= 90 ? '100' : false"
            filled
            required
          />
          <p v-t="'project.knowledgeBase.contentDescription'" />
          <v-textarea
            v-model="content"
            :rules="contentRules"
            :label="$t('project.knowledgeBase.content')"
            :counter="content.length >= 9000 ? '10000' : false"
            auto-grow
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-spacer />
          <DeleteArticleDialog
            v-if="knowledgeBaseModule.getActiveArticle"
            :article="knowledgeBaseModule.getActiveArticle"
          />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            @click.stop="save"
            :loading="loading"
            :disabled="!valid"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import KnowledgeBaseModule from '../../store/knowledgeBase'
import DeleteArticleDialog from '../../components/dialogs/DeleteArticleDialog.vue'
import { Article } from '../../models'

@Component({
  components: {
    DeleteArticleDialog,
  }
})
export default class UpdateArticleDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(Article)
  private readonly article?: Article

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private title: string = ''
  private titleRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 100 || i18n.t('core.fieldMax', { count: 100 }),
  ]
  private content: string = ''
  private contentRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 10000 || i18n.t('core.fieldMax', { count: 10000 }),
  ]

  private created() {
    if (this.article) {
      this.title = this.article.title
      this.content = this.article.content
    }
  }

  private async save() {
    if (this.article) {
      this.loading = true

      await this.knowledgeBaseModule.updateArticle({
        articleId: this.article.id,
        title: this.title,
        content: this.content,
      })

      this.loading = false
      this.dialog = false
    }
  }

}
</script>
